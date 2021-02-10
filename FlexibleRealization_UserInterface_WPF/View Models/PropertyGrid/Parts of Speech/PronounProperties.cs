using System.Collections.Generic;
using System.Linq;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="PartOfSpeechBuilder"/> in a PropertyGrid</summary>
    public class PronounProperties : ElementProperties
    {
        internal PronounProperties(PronounBuilder pb)
        {
            Model = pb;
            partOfSpeechDescription = WordBuilder.DescriptionFor(Model);
        }

        private PronounBuilder Model;

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

        [Category("Syntax|")]
        [DisplayName("Index")]
        public int Index => Model.Index;

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

        [Category("Features|Expletive Subject?")]
        [DisplayName("Specified")]
        public bool ExpletiveSubjectSpecified
        {
            get => Model.ExpletiveSubjectSpecified;
            set => Model.ExpletiveSubjectSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("ExpletiveSubjectSpecified")]
        public bool ExpletiveSubject
        {
            get => Model.ExpletiveSubject;
            set => Model.ExpletiveSubject = value;
        }

        [Category("Features|Proper?")]
        [DisplayName("Specified")]
        public bool ProperSpecified
        {
            get => Model.ProperSpecified;
            set => Model.ProperSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("ProperSpecified")]
        public bool Proper
        {
            get => Model.Proper;
            set => Model.Proper = value;
        }

        [Browsable(false)]
        public IEnumerable<string> InflectionValues => NLGElementFeature.Inflection.Strings.Values;

        [Category("Features|Inflection")]
        [DisplayName("Specified")]
        public bool InflectionSpecified
        {
            get => Model.InflectionSpecified;
            set => Model.InflectionSpecified = value;
        }

        [DisplayName("Value")]
        [ItemsSourceProperty("InflectionValues")]
        [VisibleBy("InflectionSpecified")]
        public string Inflection
        {
            get => NLGElementFeature.Inflection.Strings[Model.Inflection];
            set => Model.Inflection = NLGElementFeature.Inflection.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Canned?")]
        [DisplayName("Specified")]
        public bool CannedSpecified
        {
            get => Model.CannedSpecified;
            set => Model.CannedSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("CannedSpecified")]
        public bool Canned
        {
            get => Model.Canned;
            set => Model.Canned = value;
        }

        [Browsable(false)]
        public IEnumerable<string> CaseValues => Pronoun.CaseStrings.Values;

        [Category("Features|Case")]
        [DisplayName("Specified")]
        public bool CaseSpecified
        {
            get => Model.CaseSpecified;
            set => Model.CaseSpecified = value;
        }

        [DisplayName("Value")]
        [ItemsSourceProperty("CaseValues")]
        [VisibleBy("CaseSpecified")]
        public string Case
        {
            get => Pronoun.CaseStrings[Model.Case];
            set => Model.Case = Pronoun.CaseStrings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Person")]
        [DisplayName("Specified")]
        public bool PersonSpecified
        {
            get => Model.PersonSpecified;
            set => Model.PersonSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("PersonSpecified")]
        [ItemsSourceProperty("PersonValues")]
        public string Person
        {
            get => NLGElementFeature.Person.Strings[Model.Person];
            set => Model.Person = NLGElementFeature.Person.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Gender")]
        [DisplayName("Specified")]

        public bool GenderSpecified
        {
            get => Model.GenderSpecified;
            set => Model.GenderSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("GenderSpecified")]
        [ItemsSourceProperty("GenderValues")]
        public string Gender
        {
            get => NLGElementFeature.Gender.Strings[Model.Gender];
            set => Model.Gender = NLGElementFeature.Gender.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Number")]
        [DisplayName("Specified")]
        public bool NumberSpecified
        {
            get => Model.NumberSpecified;
            set => Model.NumberSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("NumberSpecified")]
        [ItemsSourceProperty("NumberAgreementValues")]
        public string Number
        {
            get => NLGElementFeature.Number.Strings[Model.Number];
            set => Model.Number = NLGElementFeature.Number.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        #endregion Features
    }
}