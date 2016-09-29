﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.DotCom.ProxyRequests.Owner;
using OwnApt.RestfulProxy.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Service
{
    public interface IAccountDomainService
    {
        Task<bool> ValidateSignUpTokenAsync(string token);
        Task<OwnerModel> CreateOwner(string ownerId, string ownerEmail);
        Task UpdateOwnerPropertyIds(string ownerId, string token);
        Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds);
        Task RegisterSignUpTokenAsync(string token);
    }

    public class AccountDomainService : IAccountDomainService
    {
        private readonly ILogger<AccountDomainService> logger;
        private readonly ISignUpService signUpService;
        private readonly IProxy proxy;
        private readonly ServiceUriSettings serviceUris;
        private readonly IMapper mapper;

        public AccountDomainService
        (
            ISignUpService signUpService,
            IProxy proxy,
            IMapper mapper,
            IOptions<ServiceUriSettings> serviceUriOptions,
            ILoggerFactory loggerFactory
        )
        {
            this.signUpService = signUpService;
            this.proxy = proxy;
            this.mapper = mapper;
            this.serviceUris = serviceUriOptions.Value;
            this.logger = loggerFactory.CreateLogger<AccountDomainService>();
        }

        public async Task<bool> ValidateSignUpTokenAsync(string token)
        {
            var signUpToken = await this.signUpService.ParseTokenAsync(token);
            var readRegisteredTokenRequest = new ReadRegisteredTokenProxyRequest(this.serviceUris, token);
            var readRegisteredTokenResponse = await this.proxy.InvokeAsync(readRegisteredTokenRequest);

            if (readRegisteredTokenResponse.IsSuccessfulStatusCode)
            {
                var isValid = await this.signUpService.ValidateTokenAsync(token) && readRegisteredTokenResponse.ResponseDto == null;
                return isValid;
            }

            throw ExceptionUtility.RaiseException(readRegisteredTokenResponse, this.logger);
        }

        public async Task<OwnerModel> CreateOwner(string ownerId, string ownerEmail)
        {
            var ownerModel = new OwnerModel
            {
                Id = ownerId,
                Contact = new ContactModel
                {
                    Email = ownerEmail
                }
            };

            var createOwnerRequest = new CreateOwnerProxyRequest(this.serviceUris, ownerModel);
            var createOwnerResult = await this.proxy.InvokeAsync(createOwnerRequest);

            if (createOwnerResult.IsSuccessfulStatusCode)
            {
                return createOwnerResult.ResponseDto;
            }

            throw ExceptionUtility.RaiseException(createOwnerResult, this.logger);
        }

        public async Task UpdateOwnerPropertyIds(string ownerId, string token)
        {
            var signUpToken = await this.signUpService.ParseTokenAsync(token);
            var readOwnerRequest = new ReadOwnerProxyRequest(this.serviceUris, ownerId);
            var readOwnerResponse = await this.proxy.InvokeAsync(readOwnerRequest);

            if (readOwnerResponse.IsSuccessfulStatusCode)
            {
                var ownerModel = readOwnerResponse.ResponseDto;
                ownerModel.PropertyIds.AddRange(signUpToken.PropertyIds);

                var updateOwnerRequest = new UpdateOwnerProxyRequest(this.serviceUris, ownerModel);
                var updateownerResponse = await this.proxy.InvokeAsync(updateOwnerRequest);

                if (!updateownerResponse.IsSuccessfulStatusCode)
                {
                    throw ExceptionUtility.RaiseException(updateownerResponse, this.logger);
                }
            }

            throw ExceptionUtility.RaiseException(readOwnerResponse, this.logger);
        }

        public Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds)
        {
            return this.signUpService.SendSignUpEmailAsync(name, email, propertyIds);
        }

        public async Task RegisterSignUpTokenAsync(string token)
        {
            var signUpToken = await this.signUpService.ParseTokenAsync(token);
            var registeredToken = this.mapper.Map<RegisteredTokenModel>(signUpToken);
            registeredToken.Token = token;

            var request = new CreateRegisteredTokenProxyRequest(this.serviceUris, registeredToken);
            var response = await this.proxy.InvokeAsync(request);

            if (!response.IsSuccessfulStatusCode)
            {
                throw ExceptionUtility.RaiseException(response, this.logger);
            }
        }
    }
}
