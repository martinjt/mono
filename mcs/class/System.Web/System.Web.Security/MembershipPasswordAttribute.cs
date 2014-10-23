//
// System.Web.Security.MembershipPasswordAttribute
//
// Authors:
// Martin Thwaites (github@my2cents.co.uk)
//
// (C) 2014 Martin Thwaites
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.Web.Security
{
	[AttributeUsageAttribute(AttributeTargets.Property|AttributeTargets.Field|AttributeTargets.Parameter, AllowMultiple = false)]
	public class MembershipPasswordAttribute : ValidationAttribute
	{
		public string MinNonAlphanumericCharactersError { get; set; }
		public string MinPasswordLengthError { get; set; }
		public int MinRequiredNonAlphanumericCharacters { get; set; }
		public int MinRequiredPasswordLength { get; set; }
		public string PasswordStrengthError { get; set; }
		public string PasswordStrengthRegularExpression { get; set; }
		public Type ResourceType { get; set; }

		public MembershipPasswordAttribute()
		{
			if (Membership.Provider != null)
			{
				MinRequiredNonAlphanumericCharacters = Membership.Provider.MinRequiredNonAlphanumericCharacters;
				MinRequiredPasswordLength = Membership.Provider.MinRequiredPasswordLength;
			}
			else
			{
				MinRequiredPasswordLength = 7;
				MinRequiredNonAlphanumericCharacters = 1;
			}
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var pattern = new Regex (@"\W|_");
			var strengthExpression = new Regex (PasswordStrengthRegularExpression);
			var password = value as string;

			if (MinRequiredPasswordLength > 0 && 
				password.Length < MinRequiredPasswordLength)
			{
				return new ValidationResult (
					string.Format(MinNonAlphanumericCharactersError, 
					validationContext.DisplayName,
					MinNonAlphanumericCharactersError), validationContext.MemberName);
			}

			if (MinRequiredNonAlphanumericCharacters > 0 &&
			    pattern.Match (password).Length < MinRequiredNonAlphanumericCharacters)
			{
				return new ValidationResult (
					string.Format (MinNonAlphanumericCharactersError, 
					validationContext.DisplayName, 
					MinRequiredNonAlphanumericCharacters), validationContext.MemberName);
			}

			if (!string.IsNullOrEmpty(PasswordStrengthRegularExpression) && 
				new Regex (PasswordStrengthRegularExpression).IsMatch (password))
			{
				return new ValidationResult (
					string.Format (PasswordStrengthError, 
						validationContext.DisplayName), validationContext.MemberName);
			}

			return ValidationResult.Success;
		}

		public override string FormatErrorMessage(string name)
		{
			return base.FormatErrorMessage(name);
		}
	}
}
