namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model base class for presenting an <see cref="ElementBuilder"/> in a GraphX GraphArea</summary>
    /// <remarks>An <see cref="ElementBuilder"/> can be a <see cref="ParentElementBuilder"/> or a <see cref="PartOfSpeechBuilder"/></remarks>
    internal abstract class ElementBuilderVertex : ElementVertex
    {
        /// <summary>The data model of this view model, generically typed</summary>
        internal abstract ElementBuilder Builder { get; }

        /// <summary>The IsToken property is used by XAML style triggers</summary>
        public override bool IsToken => false;
    }
}
