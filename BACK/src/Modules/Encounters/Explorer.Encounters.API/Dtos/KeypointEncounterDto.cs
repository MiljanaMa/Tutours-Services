namespace Explorer.Encounters.API.Dtos
{
    public class KeypointEncounterDto
    {
        public int Id { get; set; }
        public int EncounterId { get; set; }
        public EncounterDto Encounter { get; set; }
        public long KeypointId { get; set; }
        public bool IsRequired { get; set; }
    }
}
