using System.Collections.Generic;
using System.Text;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class ValidationError
    {
        public string MessageText { get; set; }
    }

    public class ValidationErrors
    {
        public ValidationErrors()
        {
            MessageList = new List<ValidationError>();
        }

        private List<ValidationError> MessageList { get; set; }

        public void Add(List<string> messages)
        {
            if (messages != null && messages.Count > 0)
                foreach (var message in messages)
                {
                    MessageList.Add(new ValidationError() {MessageText = message});
                }
        }

        public void Add(string message)
        {
            MessageList.Add(new ValidationError() { MessageText = message });
        }

        public void Add(ValidationError validationError)
        {
            MessageList.Add(validationError);
        }

        public int Count()
        {
            return MessageList.Count;
        }

        public bool Exists()
        {
            return MessageList.Count > 0;
        }

        public List<ValidationError> Get()
        {
            return MessageList;
        }

        public string Show()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var error in MessageList)
            {
                if (sb.ToString().Trim().Length > 0)
                {
                    sb.Append(";");
                }
                sb.Append(error.MessageText);
            }
            return sb.ToString();
        }
    }
}