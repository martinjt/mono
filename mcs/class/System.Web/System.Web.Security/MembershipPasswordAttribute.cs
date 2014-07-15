using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.Web.Security {
	[AttributeUsage (AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class MembershipPasswordAttribute : ValidationAttribute {
		private int? _minRequiredPasswordLength;
		private int? _minRequiredNonAlphanumericCharacters;
		private string _passwordStrengthRegularExpression;

		private readonly string _minRequiredPasswordLengthError = "{0} must have at least {1} characters";
		private readonly string _minNonAlphanumericCharactersError = "{0} must have at least {1} special characters";
		private readonly string _passwordStrengthError = "{0} is weak";

		public MembershipPasswordAttribute ()
		{
			MinPasswordLengthError = _minRequiredPasswordLengthError;
			MinNonAlphanumericCharactersError = _minNonAlphanumericCharactersError;
			PasswordStrengthError = _passwordStrengthError;
		}

		public int MinRequiredPasswordLength
		{
			get {
				return !_minRequiredPasswordLength.HasValue ? Membership.Provider.MinRequiredPasswordLength : _minRequiredPasswordLength.Value;
			}
			set {
				_minRequiredPasswordLength = value;
			}
		}

		public int MinRequiredNonAlphanumericCharacters
		{
			get {
				return !_minRequiredNonAlphanumericCharacters.HasValue ? Membership.Provider.MinRequiredNonAlphanumericCharacters : _minRequiredNonAlphanumericCharacters.Value;
			}
			set {
				_minRequiredNonAlphanumericCharacters = value;
			}
		}

		public string PasswordStrengthRegularExpression
		{
			get {
				return _passwordStrengthRegularExpression ?? Membership.Provider.PasswordStrengthRegularExpression;
			}
			set {
				_passwordStrengthRegularExpression = value;
			}
		}

		public Type ResourceType { get; set; }
		public string MinPasswordLengthError { get; set; }
		public string MinNonAlphanumericCharactersError { get; set; }
		public string PasswordStrengthError { get; set; }

		protected override ValidationResult IsValid (Object value, ValidationContext validationContext)
		{
			var password = (value as string) ?? string.Empty;
			var displayName = string.Empty;

			if (validationContext != null) {
				displayName = validationContext.MemberName;
				if (validationContext.DisplayName != null)
					displayName = validationContext.DisplayName;
			}

			if (string.IsNullOrEmpty (password))
				return ValidationResult.Success;

			if (password.Length < MinRequiredPasswordLength) {
				return new ValidationResult (string.Format (MinPasswordLengthError, displayName, MinRequiredPasswordLength));
			}

			if (MinRequiredNonAlphanumericCharacters > 0) {
				var nonAlphaNumCount = password.Count (c => !Char.IsLetterOrDigit (c));

				if (nonAlphaNumCount < MinRequiredNonAlphanumericCharacters) {
					return new ValidationResult (string.Format (MinNonAlphanumericCharactersError, displayName, MinRequiredPasswordLength));
				}
			}

			if (PasswordStrengthRegularExpression != null) {
				var regex = new Regex (PasswordStrengthRegularExpression);
				if (!regex.IsMatch (password)) {
					return new ValidationResult (string.Format (PasswordStrengthError, displayName, MinRequiredPasswordLength));
				}
			}

			return ValidationResult.Success;
		}
	}
}
