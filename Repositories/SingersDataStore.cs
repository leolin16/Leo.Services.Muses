using System;
using System.Collections.Generic;
using Leo.Services.Muses.Models;

namespace Leo.Services.Muses.Repositories
{
    public class SingersDataStore
    {
        public static SingersDataStore Current { get; } = new SingersDataStore();
        public List<SingerDto> Singers { get; set; }
        public SingersDataStore()
        {
            Singers = new List<SingerDto>() {
                new SingerDto() {
                    Id = 1,
                    Name = "Mario Lanza",
                    OriginName = "Alfredo Arnold Cocozza",
                    BirthDay = DateTime.Parse("1921-01-31"),
                    BriefBiography = "Mario Lanza is a great tenor with an Italian Bloodshed.",
                    DeathDate = DateTime.Parse("1959-10-07"),
                    Songs = new List<SongWithoutSingersDto>(){
                        new SongWithoutSingersDto() {
                            Id = 1,
                            Name = "Be My Love",
                            TranslatedName = "still Be My Love",
                            Author = "Peter",
                            Composer = "Nancy",
                            Language = "English",
                            Description = "first golden song"

                        },
						new SongWithoutSingersDto() {
                            Id = 2,
                            Name = "A Kiss",
                            TranslatedName = "still A Kiss",
                            Author = "Leo",
                            Composer = "Sarah",
                            Language = "English",
                            Description = "sentimental piece"

                        },
                    }
                },
                new SingerDto() {
                    Id = 2,
                    Name = "Leslie Cheung",
                    OriginName = "张国荣",
                    BirthDay = DateTime.Parse("1956-09-12"),
                    BriefBiography = "Leslie Cheung is a superstar in singing and filming.",
                    DeathDate = DateTime.Parse("2003-04-01"),
					Songs = new List<SongWithoutSingersDto>(){
						new SongWithoutSingersDto() {
                            Id = 3,
                            Name = "Count Down",
                            TranslatedName = "为你倒数",
                            Author = "Sb.",
                            Composer = "Someone",
                            Language = "Maindarin",
                            Description = "sung in 90s"

                        },
						new SongWithoutSingersDto() {
                            Id = 4,
                            Name = "夜半歌声",
                            TranslatedName = "Night Song",
                            Author = "Johnson",
                            Composer = "Shen",
                            Language = "Kantonese",
                            Description = "sung in 00s"

                        },
                    },
                },
            };
        }
    }
}
