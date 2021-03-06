using System;
using System.Collections.Generic;
using RunTogether.Dal.Model;

namespace RunTogether.ApplicationServices
{
	// Сервис уровня приложения. Выполняет утилитарную задачу по наполнению БД
	// начальными данными с предварительной очисткой. Используется в контроллере
	// "AdminController".
	public class AdminService
	{
		private string _connectionString;

		public AdminService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void RestoreData()
		{
			using (var context = new DatabaseContext(_connectionString))
			{
				foreach (var item in context.Users)
					context.Users.Remove(item);
				foreach (var item in context.Marathons)
					context.Marathons.Remove(item);

				context.SaveChanges();

				var user = new User
				{
					Name = "admin",
					Email = "admin@runtogether.ru",
					PasswordHash = "4721c2650f0a34ca39e70828d16d503a2c1ed3170a12cefaf7be3f9f225ab76a",
					PasswordSalt = "590ec3d783a2c368eb8f571b886cdea5",
					Picture = @"data:image/jpeg;base64,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",
					PictureType = "image/jpeg",
					IsAdmin = true
				};
				var springHalfMarathon = new Marathon
				{
					Title = "Весенний ежегодный полумарафон",
					Date = new DateTime(2019, 5, 1),
					Distance = 21.1F,
					Route = "{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"properties\":{},\"geometry\":{\"type\":\"LineString\",\"coordinates\":[[57.12355698172532,65.6352424621582],[57.12161173953376,65.62545776367186],[57.1231027127531,65.62288284301758],[57.121425363660826,65.61653137207031],[57.11527443367293,65.60708999633789],[57.132326774882245,65.56297302246092],[57.13642562049578,65.55267333984375],[57.13689136968934,65.5502700805664],[57.14061715226418,65.54134368896484],[57.141827950847336,65.54048538208008],[57.14359750828817,65.54117202758789],[57.1442494291732,65.54031372070312],[57.14126912565449,65.53482055664062],[57.141082848715,65.52864074707031],[57.13996516738801,65.52640914916992],[57.13884745230633,65.52297592163086],[57.13856801826167,65.5195426940918],[57.138381727726475,65.51610946655273],[57.13810229016563,65.51336288452148],[57.13633246995383,65.5030632019043],[57.13381731670835,65.49379348754883],[57.13111566545744,65.48452377319336],[57.130929337413995,65.478515625],[57.134003630216846,65.46546936035156],[57.13558725717888,65.46117782592773],[57.139126884241286,65.45722961425781],[57.141827950847336,65.45551300048828],[57.14387690435952,65.45413970947264],[57.14722949267463,65.44881820678711],[57.152350916067924,65.44212341308594],[57.155330327079454,65.44023513793945],[57.15961280998588,65.43937683105469],[57.163894797039255,65.43903350830078],[57.16547714503577,65.43903350830078],[57.16547714503577,65.44023513793945],[57.17078217063148,65.4462432861328],[57.1701307172628,65.44744491577148],[57.16854856842783,65.44795989990234],[57.1676178610119,65.44984817504883],[57.166500981184434,65.45997619628906],[57.16622175595563,65.46821594238281],[57.166128680410736,65.47130584716797],[57.164732519119845,65.47697067260742],[57.15868187762247,65.47697067260742],[57.16128842918778,65.45259475708008]]}}]}",
					WaypointInfos = new List<WaypointInfo> {
						new WaypointInfo {
							Name = "Старт",
							Description = "Стартовая позиция, \"Гилёвская роща\"",
							Location = "{\"type\":\"Feature\",\"properties\":{},\"geometry\":{\"type\":\"Point\",\"coordinates\":[57.12352203815606,65.63532829284667]}}"
						},
						new WaypointInfo {
							Name = "Финиш",
							Description = "Финишируем у парка \"Затюменского\"",
							Location = "{\"type\":\"Feature\",\"properties\":{},\"geometry\":{\"type\":\"Point\",\"coordinates\":[57.161241885235164,65.45268058776855]}}"
						}
					}
				};
				var winterHalfMarathon = new Marathon
				{
					Title = "Зимний ежегодный полумарафон",
					Date = new DateTime(2018, 1, 7),
					Distance = 21.1F
				};
				var summerMarathon = new Marathon
				{
					Title = "Летний ежегодный марафон",
					Date = new DateTime(2018, 7, 15),
					Distance = 42.195F
				};

				user.MarathonLinks.Add(new MarathonLink
				{
					Marathon = springHalfMarathon,
					User = user
				});
				user.MarathonLinks.Add(new MarathonLink
				{
					Marathon = winterHalfMarathon,
					User = user
				});
				user.MarathonLinks.Add(new MarathonLink
				{
					Marathon = summerMarathon,
					User = user
				});

				context.Users.Add(user);
				context.SaveChanges();
			}
		}
	}
}
