/**
 * By <author>mgackowski</author>.
 */

public class CardEffectArgs : IEventArgs
{
    public string EffectName { get; set; }
    public Comedian Target { get; set; }
}