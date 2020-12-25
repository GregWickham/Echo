using System.Collections.Generic;
using PropertyTools.DataAnnotations;

//namespace FlexibleRealization.UserInterface.ViewModels
//{
//    /// <summary>View Model for presenting a <see cref="PartOfSpeechBuilder"/> in a PropertyGrid</summary>
//    public class PartOfSpeechProperties : ElementProperties
//    {
//        /// <summary>Return a new view model object for <paramref name="builder"/></summary>
//        internal static PartOfSpeechProperties For(PartOfSpeechBuilder builder) => new PartOfSpeechProperties(builder);

//        private protected PartOfSpeechProperties(PartOfSpeechBuilder posb) 
//        { 
//            Model = posb;
//            partOfSpeechDescription = WordElement.DescriptionFor(Model);
//        }

//        private PartOfSpeechBuilder Model;

//        [Browsable(false)]
//        public override string Description => WordElement.DescriptionFor(Model);

//        #region Syntax

//        [Browsable(false)]
//        public IEnumerable<string> RoleValues => Parent.ChildRole.StringFormsOf(Model.ValidRolesInCurrentParent);

//        [Category("Syntax|")]
//        [DisplayName("Role")]
//        [ItemsSourceProperty("RoleValues")]
//        public string Role
//        {
//            get => Parent.ChildRole.DescriptionFrom(Model.AssignedRole);
//            set => Model.AssignedRole = Parent.ChildRole.FromDescription(value);
//        }

//        #endregion Syntax

//        #region Features

//        [Browsable(false)]
//        public IEnumerable<string> POS_DescriptionValues => PartOfSpeech.Tag.Strings.Values;

//        /// <summary>Instead of getting the partOfSpeechDescription directly from the Model, we store it in a private member.  If the user changes the public property, 
//        /// we can compare the new value against the old value to see whether it's changed.  If the user has in fact changed it, we're actually going to replace
//        /// the old Model object with a new object of a different type.</summary>
//        private string partOfSpeechDescription;
//        [Category("Features|")]
//        [DisplayName("Part of Speech")]
//        [ItemsSourceProperty("POS_DescriptionValues")]
//        public string PartOfSpeechDescription
//        {
//            get => WordElement.DescriptionFor(Model);
//            set
//            {
//                if (!partOfSpeechDescription.Equals(value))
//                {
//                    Model.ReplaceWith(WordElement.FromDescription(value));
//                }
//            }
//                //Model.PartOfSpeech = PartOfSpeech.Tag.FromDescription(value);
//        }

//        [Category("Features|")]
//        [DisplayName("POS Tag")]
//        public string Tag => Model.Token.PartOfSpeech;

//        [Category("Features|")]
//        [DisplayName("Index")]
//        public int Index => Model.Token.Index;

//        [Category("Features|")]
//        [DisplayName("Word")]
//        public string Word => Model.Token.Word;

//        [Category("Features|")]
//        [DisplayName("Lemma")]
//        public string Lemma => Model.Token.Lemma;

//        #endregion Features
//    }
//}