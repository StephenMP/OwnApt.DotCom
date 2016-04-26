using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnApt.DotCom.ViewModels.Home
{
    public class ContactFormViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string FullName => $"{this.FirstName} {this.LastName}";

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"First Name: {this.FirstName}");
            builder.AppendLine($"Last Name: {this.LastName}");
            builder.AppendLine($"Email: {this.Email}");
            builder.AppendLine($"Phone #: {this.Phone}");
            builder.AppendLine();
            builder.AppendLine($"Message:\n{this.Message}");

            return builder.ToString();
        }
    }
}
