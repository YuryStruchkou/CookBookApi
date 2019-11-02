namespace CookBook.Domain.ResultDtos.RecipeDtos
{
    public class CurrentUserVoteDto
    {
        public int? VoteValue { get; set; }

        public CurrentUserVoteDto(int? voteValue)
        {
            VoteValue = voteValue;
        }
    }
}
