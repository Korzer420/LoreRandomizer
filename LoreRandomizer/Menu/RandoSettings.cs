namespace LoreRandomizer.Menu;

public class RandoSettings
{
    #region Properties

    public bool Enabled { get; set; }

    public bool UseCustomLore { get; set; }

    public bool RandomizeNpc { get; set; }

    public bool RandomizeDreamNailDialogue { get; set; }

    public bool RandomizePointOfInterest { get; set; }

    public bool RandomizeTravellerDialogues { get; set; }

    public TravellerBehaviour TravellerOrder { get; set; }

    public bool RandomizeElderbugRewards { get; set; }

    public bool RandomizeShrineOfBelievers { get; set; }

    public bool CursedReading { get; set; }

    public bool CursedListening { get; set; }

    #endregion
}
