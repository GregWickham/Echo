using System.Collections.Generic;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="PartOfSpeechBuilder"/> in a PropertyGrid</summary>
    public class WordPartOfSpeechProperties : ElementProperties
    {
        /// <summary>Return a new view model object for <paramref name="builder"/></summary>
        internal static WordPartOfSpeechProperties For(WordElementBuilder builder) => new WordPartOfSpeechProperties(builder);

        private WordPartOfSpeechProperties(WordElementBuilder web)
        {
            Model = web;
            partOfSpeechDescription = WordBuilder.DescriptionFor(Model);
        }

        private WordElementBuilder Model;

        [Browsable(false)]
        public override string Description => WordBuilder.DescriptionFor(Model);

        #region Syntax

        [Browsable(false)]
        public IEnumerable<string> RoleValues => Parent.ChildRole.StringFormsOf(Model.ValidRolesInCurrentParent);

        [Category("Syntax|")]
        [DisplayName("Role")]
        [ItemsSourceProperty("RoleValues")]
        public string Role
        {
            get => Parent.ChildRole.DescriptionFrom(Model.AssignedRole);
            set => Model.AssignedRole = Parent.ChildRole.FromDescription(value);
        }

        #endregion Syntax

        #region Features

        [Browsable(false)]
        public IEnumerable<string> POS_DescriptionValues => WordBuilder.PartOfSpeechDescriptions;

        /// <summary>Instead of getting the partOfSpeechDescription directly from the Model, we store it in a private member.  If the user changes the public property, 
        /// we can compare the new value against the old value to see whether it's changed.  If the user has in fact changed it, we're actually going to replace
        /// the old Model object with a new object of a different type.</summary>
        private string partOfSpeechDescription;
        [Category("Features|")]
        [DisplayName("Part of Speech")]
        [ItemsSourceProperty("POS_DescriptionValues")]
        public string PartOfSpeechDescription
        {
            get => WordBuilder.DescriptionFor(Model);
            set
            {
                if (!partOfSpeechDescription.Equals(value))
                {
                    Model.ReplaceWith(WordBuilder.FromDescription(value));
                }
            }
        }

        [Category("Features|")]
        [DisplayName("Part of Speech")]
        public string Tag => WordBuilder.DescriptionFor(Model);

        [Category("Features|")]
        [DisplayName("Index")]
        public int Index => Model.Index;

        #endregion Features
    }
}