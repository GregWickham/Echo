using System;

namespace FlexibleRealization.UserInterface.ViewModels
{
    public abstract class ElementProperties 
    {
        public abstract string Description { get; }

        internal static ElementProperties For(ElementBuilder builder) => builder switch
        {
            WordElementBuilder web => WordPartOfSpeechProperties.For(web),
            ParentElementBuilder peb => ParentProperties.For(peb),
            _ => throw new InvalidOperationException("No properties view model for this ElementBuilder type")
        };
    }
}
