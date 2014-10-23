using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using NUnit.Framework;

namespace MonoTests
{
	[TestFixture]
	public class MembershipPasswordAttributeTest
	{
		private ValidationContext _validationContext;

		public MembershipPasswordAttributeTest()
		{
			_validationContext = new ValidationContext(new object())
				{
					DisplayName = "testDisplay",
					MemberName = "testMember"
				};
		}

		[Test]
		public void MinRequiredPasswordLength ()
		{
			var passwordAttribute = new MembershipAttributeTest ();
			ValidationResult result = passwordAttribute.TestValidation ("a!12345", _validationContext);
			Assert.AreEqual (ValidationResult.Success, result, "Should suceed with a 7 character length");

			result = passwordAttribute.TestValidation ("a!1234", _validationContext);
			Assert.AreEqual ("The 'testDisplay' field is an invalid password. Password must have 7 or more characters.", result.ErrorMessage, "Error message not correct for lower Min characters");
			Assert.AreEqual (_validationContext.MemberName, result.MemberNames.FirstOrDefault (), "Member name not correct");

			passwordAttribute.MinRequiredPasswordLength = 6;
			result = passwordAttribute.TestValidation ("a!1234", _validationContext);
			Assert.AreEqual (ValidationResult.Success, result, "Should suceed with a 6 character length after it's reset");
		}

		internal class MembershipAttributeTest : MembershipPasswordAttribute
		{
			public ValidationResult TestValidation(object val, ValidationContext context)
			{
				return IsValid (val, context);
			}
		}
	}
}
