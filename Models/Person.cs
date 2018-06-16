using System.ComponentModel.DataAnnotations;

namespace exam70486
{
    public class Person
    {
        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}", ErrorMessage = "Invalid Email address")]
        public string Email { get; set; }
    }
}