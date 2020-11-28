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


        [Category("Features|")]
        [DisplayName("POS Tag")]
        public string Tag => Model.Token.PartOfSpeech;

        [Category("Features|")]
        [DisplayName("Index")]
        public int Index => Model.Token.Index;

        [Category("Features|")]
        [DisplayName("Word")]
        public string Word => Model.Token.Word;

        [Category("Features|")]
        [DisplayName("Lemma")]
        public string Lemma => Model.Token.Lemma;
    }
}