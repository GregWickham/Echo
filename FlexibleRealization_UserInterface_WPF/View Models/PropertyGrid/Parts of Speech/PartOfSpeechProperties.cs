using System.ComponentModel;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="PartOfSpeechBuilder"/> in a PropertyGrid</summary>
    public class PartOfSpeechProperties : ElementProperties
    {
        internal static PartOfSpeechProperties For(PartOfSpeechBuilder builder) => new PartOfSpeechProperties(builder);

        private protected PartOfSpeechProperties(PartOfSpeechBuilder posb) { Model = posb; }

        private PartOfSpeechBuilder Model;

        [Browsable(false)]
        public override string Description => PartOfSpeech.DescriptionFor(Model);

        [Category("Syntax|")]
        [DisplayName("Role")]
        public string Role => Parent.DescriptionFor(Model.AssignedRole);


        [Category("Properties|")]
        [DisplayName("POS Tag")]
        public string Tag => Model.Token.PartOfSpeech;

        [Category("Properties|")]
        [DisplayName("Index")]
        public int Index => Model.Token.Index;

        [Category("Properties|")]
        [DisplayName("Word")]
        public string Word => Model.Token.Word;

        [Category("Properties|")]
        [DisplayName("Lemma")]
        public string Lemma => Model.Token.Lemma;
    }
}