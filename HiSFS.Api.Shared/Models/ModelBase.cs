using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS0067
namespace HiSFS.Api.Shared.Models
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [Column(Order = 600)]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [Column(Order = 601)]
        [MaxLength(50)]
        public string CreateId { get; set; } = "SYSTEM";
        [Column(Order = 602)]
        public DateTime? UpdateTime { get; set; } = DateTime.Now;
        [MaxLength(50)]
        [Column(Order = 603)]
        public string UpdateId { get; set; } = "SYSTEM";

        [NotMapped]
        [JsonIgnore]
        public int No { get; set; }
    }

    public static class ModelExtension
    {
        //        list = new ObservableCollection<직원정보>(await this.Remote.Command.기준정보.직원정보_조회());

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> @this)
            where T : ModelBase
        {
            return new ObservableCollection<T>(@this);
        }
    }
}
