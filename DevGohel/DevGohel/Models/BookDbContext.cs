using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;
namespace DevGohel.Models
{
    public class BookDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TData> TDatas { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Sponser> Sponsers { get; set; }
        public DbSet<TDataFile> TDataFiles { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
    }

    public class Author
    {
        public long AuthorId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime Created { get; set; }
        [Required(ErrorMessage = "*")]
        public bool IsActive { get; set; }

        public ICollection<Topic> Topics { get; set; }
    }

    public class Label
    {
        public long LabelId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string EName { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime Created { get; set; }
        [Required(ErrorMessage = "*")]
        public bool IsActive { get; set; }

        public ICollection<Topic> Topics { get; set; }
    }

    public class Topic
    {
        public long TopicId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string Ename { get; set; }
        public TopicType TopicType { get; set; }
        public BgColor BgColor { get; set; }
        public TxColor TxColor { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime Created { get; set; }
        [Required(ErrorMessage = "*")]
        public bool IsActive { get; set; }

        [ForeignKey("Label")]
        public long LabelId { get; set; }
        public Label Label { get; set; }

        [ForeignKey("Author")]
        public long AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<TData> TDatas { get; set; }
        public ICollection<Visitor> Visitors { get; set; }
    }

    public class TData
    {
        public long TdataId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "*")]
        public string Body { get; set; }
        [Required(ErrorMessage = "*")]
        public TDataType Type { get; set; }
        public string ExtraText { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime Created { get; set; }
        [Required(ErrorMessage = "*")]
        public bool IsActive { get; set; }

        [ForeignKey("Topic")]
        public long TopicId { get; set; }
        public Topic Topic { get; set; }

        public int OrderId { get; set; }
    }

    public class Visitor
    {
        public long VisitorId { get; set; }
        public string IpAddress { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        [ForeignKey("Topic")]
        public long TopicId { get; set; }
        public Topic Topic { get; set; }
    }

    public class Sponser
    {
        public int SponserId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "*")]
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }

    public class TDataFile
    {
        public long TDataFileId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
    }


    public class FeedBack
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(30, ErrorMessage = "<30")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "*")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "X")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "*")]
        public string City { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(500, ErrorMessage = "X")]
        public string Body { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }
    }

    public class UploadFile
    {
        public long UploadFileId { get; set; }
        [Required(ErrorMessage = "*")]
        public string FileName { get; set; }
        [Required(ErrorMessage = "*")]
        public string FilePath { get; set; }
        [Required(ErrorMessage = "*")]
        public FileExtesion FExtension { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
    }

    public class ErrorList
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public string Message { get; set; }
    }

    public enum FileExtesion
    {
        PDF,
        TXT,
        DOC,
        DOCX,
        PPT,
        PPTX,
        ZIP,
        RAR
    }
    public enum TopicType
    {
        Topic,
        Author
    }
    public enum BgColor
    {
        CornflowerBlue,
        DarkSlateBlue,
        LightGreen,
        MidnightBlue
    }
    public enum TxColor
    {
        Black,
        White,
        Gray
    }
    public enum TDataType
    {
        HtmlText,
        Website,
        Youtube,
        Files
    }

}