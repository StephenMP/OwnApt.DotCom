using System.Net;
using System.Threading.Tasks;
using DotCom.Tests.Component.TestingUtilities;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Dto.Account;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
    public class SignUpServiceGiven : Given
    {
        #region Private Fields

        private SignUpServiceSteps signUpServiceSteps;

        #endregion Private Fields

        #region Public Constructors

        public SignUpServiceGiven(SignUpServiceSteps steps) : base(steps)
        {
            this.signUpServiceSteps = steps;
        }

        #endregion Public Constructors

        #region Public Methods

        public void IHaveAMockedSignUpService()
        {
            var mailGunRestClient = MailGunRestClientMockBuilder
                                        .New()
                                        .ExecuteAny(HttpStatusCode.OK)
                                        .Build();

            var signUpLoggerFactory = LoggerFactoryMockBuilder
                                        .New()
                                        .Build();

            this.signUpServiceSteps.signupService = new SignUpService(mailGunRestClient, signUpLoggerFactory);
        }

        public void IHaveAPropertyId()
        {
            this.signUpServiceSteps.propertyId = TestRandom.String;
        }

        public async Task IHaveATokenToParse()
        {
            this.signUpServiceSteps.token = await this.signUpServiceSteps.signupService.CreateTokenAsync(this.signUpServiceSteps.propertyId);
        }

        #endregion Public Methods
    }

    public class SignUpServiceSteps : Steps
    {
        #region Internal Fields

        internal string propertyId;
        internal ISignUpService signupService;
        internal SignUpTokenDto signUpToken;
        internal string token;

        #endregion Internal Fields

        #region Public Constructors

        public SignUpServiceSteps()
        {
            this.Given = new SignUpServiceGiven(this);
            this.When = new SignUpServiceWhen(this);
            this.Then = new SignUpServiceThen(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public SignUpServiceGiven Given { get; }
        public SignUpServiceThen Then { get; }
        public SignUpServiceWhen When { get; }

        #endregion Public Properties
    }

    public class SignUpServiceThen : Then
    {
        #region Private Fields

        private SignUpServiceSteps signUpServiceSteps;

        #endregion Private Fields

        #region Public Constructors

        public SignUpServiceThen(SignUpServiceSteps steps) : base(steps)
        {
            this.signUpServiceSteps = steps;
        }

        #endregion Public Constructors

        #region Public Methods

        public void ICanVerifyICanParseSignUpToken()
        {
            Assert.NotNull(this.signUpServiceSteps.signUpToken);
            Assert.Equal(this.signUpServiceSteps.propertyId, this.signUpServiceSteps.signUpToken.PropertyIds[0]);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void ICanVerifyICreateToken()
        {
            Assert.NotNull(this.signUpServiceSteps.token);
            Assert.NotEmpty(this.signUpServiceSteps.token);
        }

        #endregion Internal Methods
    }

    public class SignUpServiceWhen : When
    {
        #region Private Fields

        private SignUpServiceSteps signUpServiceSteps;

        #endregion Private Fields

        #region Public Constructors

        public SignUpServiceWhen(SignUpServiceSteps steps) : base(steps)
        {
            this.signUpServiceSteps = steps;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task ICreateSignUpToken()
        {
            this.signUpServiceSteps.token = await this.signUpServiceSteps.signupService.CreateTokenAsync(this.signUpServiceSteps.propertyId);
        }

        public async Task IParseSignUpTokenAsync()
        {
            this.signUpServiceSteps.signUpToken = await this.signUpServiceSteps.signupService.ParseTokenAsync(this.signUpServiceSteps.token);
        }

        #endregion Public Methods
    }
}
