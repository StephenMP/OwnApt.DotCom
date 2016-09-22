using System.Text;

namespace OwnApt.DotCom.ViewModels.Dto
{
    public class ContactFormViewDto
    {
        #region Public Properties

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string FullName => $"{this.FirstName} {this.LastName}";
        public string LastName { get; set; }
        public string Message { get; set; }
        public string Phone { get; set; }

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}
