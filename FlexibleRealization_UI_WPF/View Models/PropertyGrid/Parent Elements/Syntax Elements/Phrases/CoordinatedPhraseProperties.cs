using System.Collections.Generic;
using System.Linq;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="CoordinatedPhraseBuilder"/> in a PropertyGrid</summary>
    //public class CoordinatedPhraseProperties : ParentProperties
    //{
    //    internal CoordinatedPhraseProperties(CoordinatedPhraseBuilder cpb) : base(cpb) { Model = cpb; }

    //    private CoordinatedPhraseBuilder Model;

    //    #region Syntax

    //    [Browsable(false)]
    //    public IEnumerable<string> RoleValues => Parent.ChildRole.StringFormsOf(Model.ValidRolesInCurrentParent);

    //    [Category("Syntax|")]
    //    [DisplayName("Role")]
    //    [ItemsSourceProperty("RoleValues")]
    //    public string Role
    //    {
    //        get => Parent.ChildRole.DescriptionFrom(Model.AssignedRole);
    //        set => Model.AssignedRole = Parent.ChildRole.FromDescription(value);
    //    }

    //    [Category("Syntax|")]
    //    [DisplayName("Start Index")]
    //    public int MinimumIndex => Model.MinimumIndex;

    //    [Category("Syntax|")]
    //    [DisplayName("End Index")]
    //    public int MaximumIndex => Model.MaximumIndex;

    //    #endregion Syntax

    //    #region Features

    //    [Category("Features|Conjunction")]
    //    [DisplayName("Word")]
    //    public string Conjunction
    //    {
    //        get => Model.Conjunction;
    //        set => Model.Conjunction = value;
    //    }

    //    [Category("Features|Appositive?")]
    //    [DisplayName("Specified")]
    //    public bool AppositiveSpecified
    //    {
    //        get => Model.AppositiveSpecified;
    //        set => Model.AppositiveSpecified = value;
    //    }

    //    [DisplayName("Value")]
    //    [VisibleBy("AppositiveSpecified")]
    //    public bool Appositive
    //    {
    //        get => Model.Appositive;
    //        set => Model.Appositive = value;
    //    }

    //    [Category("Features|Conjunction Type")]
    //    [DisplayName("Word")]
    //    public string ConjunctionType
    //    {
    //        get => Model.ConjunctionType;
    //        set => Model.ConjunctionType = value;
    //    }

    //    [Category("Features|Modal")]
    //    [DisplayName("Word")]
    //    public string Modal
    //    {
    //        get => Model.Modal;
    //        set => Model.Modal = value;
    //    }

    //    [Category("Features|Number")]
    //    [DisplayName("Specified")]
    //    public bool NumberSpecified
    //    {
    //        get => Model.NumberSpecified;
    //        set => Model.NumberSpecified = value;
    //    }

    //    [DisplayName("Value")]
    //    [VisibleBy("NumberSpecified")]
    //    [ItemsSourceProperty("NumberAgreementValues")]
    //    public string Number
    //    {
    //        get => NLGElementFeature.Number.Strings[Model.Number];
    //        set => Model.Number = NLGElementFeature.Number.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
    //    }

    //    [Category("Features|Person")]
    //    [DisplayName("Specified")]
    //    public bool PersonSpecified
    //    {
    //        get => Model.PersonSpecified;
    //        set => Model.PersonSpecified = value;
    //    }

    //    [DisplayName("Value")]
    //    [VisibleBy("PersonSpecified")]
    //    [ItemsSourceProperty("PersonValues")]
    //    public string Person
    //    {
    //        get => NLGElementFeature.Person.Strings[Model.Person];
    //        set => Model.Person = NLGElementFeature.Person.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
    //    }

    //    [Category("Features|Possessive?")]
    //    [DisplayName("Specified")]
    //    public bool PossessiveSpecified
    //    {
    //        get => Model.PossessiveSpecified;
    //        set => Model.PossessiveSpecified = value;
    //    }

    //    [DisplayName("Value")]
    //    [VisibleBy("PossessiveSpecified")]
    //    public bool Possessive
    //    {
    //        get => Model.Possessive;
    //        set => Model.Possessive = value;
    //    }

    //    [Category("Features|Progressive?")]
    //    [DisplayName("Specified")]
    //    public bool ProgressiveSpecified
    //    {
    //        get => Model.ProgressiveSpecified;
    //        set => Model.ProgressiveSpecified = value;
    //    }

    //    [DisplayName("Value")]
    //    [VisibleBy("ProgressiveSpecified")]
    //    public bool Progressive
    //    {
    //        get => Model.Progressive;
    //        set => Model.Progressive = value;
    //    }

    //    [Category("Features|Raise Specifier?")]
    //    [DisplayName("Specified")]
    //    public bool RaiseSpecifierSpecified
    //    {
    //        get => Model.RaiseSpecifierSpecified;
    //        set => Model.RaiseSpecifierSpecified = value;
    //    }

    //    [DisplayName("Value")]
    //    [VisibleBy("RaiseSpecifierSpecified")]
    //    public bool RaiseSpecifier
    //    {
    //        get => Model.RaiseSpecifier;
    //        set => Model.RaiseSpecifier = value;
    //    }

    //    [Category("Features|Suppressed Complementiser?")]
    //    [DisplayName("Specified")]
    //    public bool SuppressedComplementiserSpecified
    //    {
    //        get => Model.SuppressedComplementiserSpecified;
    //        set => Model.SuppressedComplementiserSpecified = value;
    //    }

    //    [DisplayName("Value")]
    //    [VisibleBy("SuppressedComplementiserSpecified")]
    //    public bool SuppressedComplementiser
    //    {
    //        get => Model.SuppressedComplementiser;
    //        set => Model.SuppressedComplementiser = value;
    //    }

    //    [Category("Features|Tense")]
    //    [DisplayName("Specified")]
    //    public bool TenseSpecified
    //    {
    //        get => Model.TenseSpecified;
    //        set => Model.TenseSpecified = value;
    //    }

    //    [DisplayName("Value")]
    //    [VisibleBy("TenseSpecified")]
    //    [ItemsSourceProperty("TenseValues")]
    //    public string Tense
    //    {
    //        get => NLGElementFeature.Tense.Strings[Model.Tense];
    //        set => Model.Tense = NLGElementFeature.Tense.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
    //    }

    //    #endregion Features
    //}
}