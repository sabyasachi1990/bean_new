using Repository.Pattern.Ef6;

namespace AppsWorld.ReminderModule.Entities.V2Entities
{
    public class SOAReminderBatchListDetailsEntity : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid MasterId { get; set; }
        public decimal? CreditNoteBalance { get; set; }

    }
}
