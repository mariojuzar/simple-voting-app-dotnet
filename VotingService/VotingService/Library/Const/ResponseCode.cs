namespace VotingService.Library.Const
{
    public enum ResponseCode
    {
        DATA_NOT_EXIST,
        DATA_ALREADY_EXIST,
        USER_NOT_EXIST,
        VOTING_ITEM_NOT_EXIST_OR_EXPIRED,
        VOTING_ITEM_DUE_DATE_NOT_VALID,
        USER_ALREADY_VOTED,
        FAILED_TO_VOTE,
        FAILED_TO_CREATED_DATA,
        FAILED_TO_DELETE_DATA,
        FAILED_TO_UPDATE_DATA
    }
}
    