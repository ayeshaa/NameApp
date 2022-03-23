using System.Text.RegularExpressions;

namespace NameApp.Service
{
    public class ValidateInput : IValidateInput
    {
        public bool IsNameValid(string name)
        {
            return Regex.IsMatch(name, "^[a-z ]+$");
        }
    }
}
