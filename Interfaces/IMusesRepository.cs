using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Leo.Services.Muses.Entities;
using Leo.Services.Muses.Models;

namespace Leo.Services.Muses.Interfaces
{
	public interface IMusesRepository
	{
		Task<bool> SingerExistsAsync(int id);
		Task<bool> SingerExistsAsync(string name);
		Task<IEnumerable<Singer>> GetSingersAsync();
		//Task<IEnumerable<Singer>> GetSingersAsync(int songId);
		//Task<IEnumerable<Singer>> GetSingersAsync(string songName);
		Task<Singer> GetSingerAsync(int id, bool includeSongs);
		Task<Singer> GetSingerAsync(string name, bool includeSongs);

      

		Task<bool> SongExistsAsync(int id);
        Task<bool> SongExistsAsync(string name);
		Task<IEnumerable<Song>> GetSongsAsync();
		//Task<IEnumerable<Song>> GetSongsAsync(int singerId);
		//Task<IEnumerable<Song>> GetSongsAsync(string singerName);
		Task<Song> GetSongAsync(int id, bool includeSingers);
		Task<Song> GetSongAsync(string name, bool includeSingers);

		Task AddSongAsync(Song song);
		Task AddSongForSingerAsync(int singerId, Song song);
		Task AddSongForSingerAsync(string singerName, Song song);
		Task DeleteSongAsync(Song song);
		//Task<IEnumerable<Song>> GetSingerSongsAsync(int SingerId);
		//Task<IEnumerable<Song>> GetSingerWithSongsAsync(int SingerId);
		//Task<IEnumerable<Song>> GetSongSingersAsync(int SongId);
		//Task<IEnumerable<Song>> GetSongWithSingersAsync(int SongId);


		Task<bool> CriticismExistsAsync(int id);
        Task<bool> CriticismExistsAsync(CriticismDtoForEdit criticism);
		Task AddCriticismForSingerAsync(int singerId, Criticism criticism);
		Task AddCriticismForSingerAsync(string singerName, Criticism criticism);
		Task DeleteCriticismAsync(Criticism criticism);
		Task<bool> SaveAsync();
	}
}
    