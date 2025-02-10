using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Model.QCComment
{
    public class QCRatingViewModel
    {
        public int QcratingId { get; set; }
        public int ProductCatId { get; set; }
        public string? Qcquestion { get; set; }
        public int RatingWeightage { get; set; }
        public int QuestionerLovid { get; set; }
        public string? QuestionerLOVName { get; set; }
        public int? Condition { get; set; }
        public double AverageSellingPrice { get; set; }
        public decimal Sweetner { get; set; }
        public string? CommentByQC { get; set; }
        // Added by VK for Diagnose V2
        public int? QcratingMasterMappingId { get; set; }
        public bool? IsAgeingQues { get; set; }
        public bool? IsDecidingQues { get; set; }
        public bool? IsDiagnoseV2 { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductTechnologyId { get; set; }
        public int? QuestsSequence { get; set; }
        public SelectList? OptionsList { get; set; }
        public List<QuestionerLovMappingViewModel>? questionerLovMappingViewModel { get; set; }
    }

    public class QCRatingLOVDataViewModel
    {
        public int QcratingId { get; set; }
        public int ProductCatId { get; set; }
        public string? Qcquestion { get; set; }
        public int RatingWeightage { get; set; }
        public int QuestionerLovid { get; set; }
        public List<QuestionerLovidViewModel>? questionerLovidViewModels { get; set; }
        public int Condition { get; set; }
        public double AverageSellingPrice { get; set; }
        public decimal Sweetner { get; set; }
        public string? CommentByQC { get; set; }
        public string? QuestionsImage { get; set; }
        public bool? IsAgeingQues { get; set; }
        public bool? IsDecidingQues { get; set; }
        public List<QuestionerLovMappingViewModel>? questionerLovMappingViewModel { get; set; }
    }
    public class QuestionerLovidViewModel
    {
        public int QuestionerLovid { get; set; }
        public string? QuestionerLovname { get; set; }
        public int? QuestionerLovparentId { get; set; }
    }
    public class QuestionerLovMappingViewModel
    {
        public int QuestionerLovmappingId { get; set; }
        public int ProductCatId { get; set; }
        public int QuestionerLovid { get; set; }
        public int? ParentId { get; set; }
        public decimal? RatingWeightageLov { get; set; }
        public string? QuestionerLovname { get; set; }
    }
    public class QcratingMasterMappingVM
    {
        public int QcratingMasterMappingId { get; set; }
        public int? QcratingId { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductTechnologyId { get; set; }
        public int? QuestsSequence { get; set; }
        public List<QCRatingLOVDataViewModel>? qCRatingLOVDataViewModels { get; set; }
       
    }
    // Added by VK for Get List of Questions
    public class QuestionsWithLovViewModel
    {
        public int QcratingMasterMappingId { get; set; }
        public int? QcratingId { get; set; }
        public string? Qcquestion { get; set; }
        public int? RatingWeightage { get; set; }
        public int? QuestionerLovid { get; set; }
        public bool? IsAgeingQues { get; set; }
        public bool? IsDecidingQues { get; set; }
        public bool? IsDiagnoseV2 { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductTechnologyId { get; set; }
        public int? QuestsSequence { get; set; }
        public SelectList? OptionsList { get; set; }
        public List<QuestionerLovMappingViewModel>? questionerLovMappingViewModel { get; set; }
    }
}
