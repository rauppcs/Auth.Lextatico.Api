namespace Auth.Lextatico.Application.Dtos
{
    public abstract class BaseDto
    {
        public Guid Id { get; set; }
        private DateTime CreatedAt { get; set; }
        private DateTime? UpdatedAt { get; set; }
    }
}
