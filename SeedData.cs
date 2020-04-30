using System;
using System.Collections.Generic;
using System.Linq;
using Leo.Services.Muses.Data;
using Leo.Services.Muses.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Leo.Services.Muses
{
    public static class SeedData
    {
		public static IHost EnsureSeedDataForMuses(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
				var serviceProvider = scope.ServiceProvider;
				var context = serviceProvider.GetService<MusesDbContext>();
                context.Database.Migrate();
                
				if (context.Singers.Any() && context.Songs.Any())
                {
					Console.WriteLine("Singer and Song already exist");
                }
                else
                {

					var singers = new List<Singer>() {
                        new Singer() {
                            Name = "Mario Lanza",
                            OriginName = "Alfredo Arnold Cocozza",
                            BirthDay = DateTime.Parse("1921-01-31"),
                            BriefBiography = "Mario Lanza is a great tenor with an Italian Bloodshed.",
                            DeathDate = DateTime.Parse("1959-10-07"),
							Criticisms = new List<Criticism>() {
								new Criticism() {
									Critic = "Leo Lin",
									Opinion = "Best Singer of All Time",
								},
								new Criticism() {
									Critic = "Somebody",
									Opinion = "My Favorite",
								},
							},
                        },
                        new Singer() {
                            Name = "Leslie Cheung",
                            OriginName = "张国荣",
                            BirthDay = DateTime.Parse("1956-09-12"),
                            BriefBiography = "Leslie Cheung is a superstar in singing and filming.",
                            DeathDate = DateTime.Parse("2003-04-01"),
							Criticisms = new List<Criticism>() {
								new Criticism() {
									Critic = "Somebody",
									Opinion = "Chinese gets high?",
								},
								new Criticism() {
									Critic = "Leo Lin",
									Opinion = "Unique of 2 decades",
								},
							},
                        },
                    };
					var songs = new List<Song>(){
						new Song() {
							Name = "Be My Love",
							TranslatedName = "still Be My Love",
							Author = "Peter",
							Composer = "Nancy",
							Language = "English",
							Description = "first golden song"

						},
						new Song() {
							Name = "A Kiss",
							TranslatedName = "still A Kiss",
							Author = "Leo",
							Composer = "Sarah",
							Language = "English",
							Description = "sentimental piece"

						},
						new Song() {
							Name = "Count Down",
							TranslatedName = "为你倒数",
							Author = "Sb.",
							Composer = "Someone",
							Language = "Maindarin",
							Description = "sung in 90s"

						},
						new Song() {
							Name = "夜半歌声",
							TranslatedName = "Night Song",
							Author = "Johnson",
							Composer = "Shen",
							Language = "Kantonese",
							Description = "sung in 00s"

						},
					};
					var singerSongs = new List<SingerSong>()
					{
						new SingerSong() {Singer = singers[0], Song = songs[0]},
						new SingerSong() {Singer = singers[0], Song = songs[1]},
						new SingerSong() {Singer = singers[1], Song = songs[2]},
						new SingerSong() {Singer = singers[1], Song = songs[3]},
					};
					context.AddRange(singerSongs);
					context.SaveChanges();

                    Console.WriteLine("Singers and Songs created");
                }
			}
			return host;
        }
    }
}
