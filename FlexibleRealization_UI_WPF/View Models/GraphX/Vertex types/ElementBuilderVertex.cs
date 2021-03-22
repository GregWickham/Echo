namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model base class for presenting an ElementBuilder in a GraphX GraphArea</summary>
    /// <remarks>An ElementBuilder can be a ParentElementBuilder or a PartOfSpeechBuilder</remarks>
    internal abstract class ElementBuilderVertex : ElementVertex
    {
        /// <summary>The data model of this view model, generically typed</summary>
        internal abstract ElementBuilder Builder { get; }

        /// <summary>The IsToken property is used by XAML style triggers</summary>
        public override bool IsWordContents => false;
    }
}
