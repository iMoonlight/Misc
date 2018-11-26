using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EFT;

namespace ENBoost
{
	public class RAMbooster : MonoBehaviour
	{
		public RAMbooster()
		{
		}
		
		GameObject objectHolder;
		
		List<Player> playersArray = new List<Player>();
		List<Grenade> grenadesArray = new List<Grenade>();
		List<LootItem> lootArray = new List<LootItem>();
		List<LootableContainer> lootBoxesArray = new List<LootableContainer>();
		List<ExfiltrationPoint> exitsArray = new List<ExfiltrationPoint>();
		
		bool texturesPreperaed;
		
		int playersCount;
		
		bool isShowPlayers;
		bool isShowCorpses;
		bool isShowBones;
		bool isShowGrenades;
		bool isShowLoot;
		bool isShowLootBoxes;
		bool isShowExits;
		
		bool isShowLootMisc;
		bool isShowLootMeds;
		bool isShowLootGuns;
		bool isShowLootAddons;
		
		bool isShowLootBoxGood;
		
		bool isShowScavExits;

		bool menuTabESP;
		bool menuTabLootFilter;
		bool menuSelfShow;
		
		float playersNextUpdateTime;
		float grenadesNextUpdateTime;
		float lootNextUpdateTime;
		float exitsNextUpdateTime;
		
		const float updateInterval = 2.0f;
		const int updatePlayersIntervalMul = 3;
		const int updateGrenadesIntervalMul = 1;
		const int updateLootIntervalMul = 4;
		const int updateExitsIntervalMul = 4;
		
		const float scanDistance = 850f;
		const float drawDistance = 800f;
		
		string[] lootFilterMisc = {
			"powder",
			"case",
			"key",
			"dorm",
			"watch",
			"lion",
			"cat",
			"gas",
			"bitcoin",
			"video",
			"chain",
			"cpu",
			"gphone",
			"dvl",
			"card",
			"dogtag",
			"horse",
			"clock",
			"clin",
			"lamp",
			"geiger",
			"window",
			"gm"
		};
		
		readonly string[] lootFilterMeds = {
			"grizzly",
			"morphine",
			"salewa",
			"aid",
			"kit",
			"analgin"
		};
		readonly string[] lootFilterGuns = { "weapon", "ammo" };
		readonly string[] lootFilterAddons = {
			"supressor",
			"grip",
			"scope",
			"sight",
			"mod",
			"stock",
			"receiver",
			"optic",
			"cover",
			"barrel",
			"magazine"
		};
		
		readonly string[] lootBoxFilterGood = { "weapon", "ammo", "cover" };

		public void Begin()
		{
			objectHolder = new GameObject();
			objectHolder.AddComponent<RAMbooster>();

			DontDestroyOnLoad(objectHolder);
		}
		
		void Prepeare()
		{
			GhUI.PrepeareTexture("red", Color.red);
			GhUI.PrepeareTexture("blue", Color.blue);			
			GhUI.PrepeareTexture("yellow", Color.yellow);
			
			GhUI.PrepeareTexture("white", Color.white);
			
//			GhUI.PrepeareTexture("cyan", Color.cyan);
//			GhUI.PrepeareTexture("green", Color.green);
			
			texturesPreperaed = true;
		}
		
		void Unload()
		{
			Destroy(objectHolder);
			
			GhUI.DestroyTexture("red");
			GhUI.DestroyTexture("blue");
			GhUI.DestroyTexture("yellow");
			
			GhUI.DestroyTexture("white");
			
//			GhUI.DestroyTexture("cyan");
//			GhUI.DestroyTexture("green");
			
			Destroy(this);
		}

		void Update()
		{
			if (Input.GetKeyUp(KeyCode.Delete)) {
				Unload();
			}
			if (Input.GetKeyUp(KeyCode.Home)) {
				menuSelfShow = !menuSelfShow;
				if (!texturesPreperaed)
					Prepeare();
			}
		}

		void OnGUI()
		{			
			if (menuSelfShow) {
				menuSelfShowM();
			}						
			
			try {
				if (isShowPlayers && Time.time >= playersNextUpdateTime) {
					playersArray.Clear();
					playersArray = FindObjectsOfType<Player>().Where(s => (isInScanZoneP(s.Transform) && (PlayerFilter(s)))).ToList();
					playersCount = playersArray.Count - 1;
					playersNextUpdateTime = Time.time + updateInterval * updatePlayersIntervalMul;
				}
			
				if (isShowGrenades && Time.time >= grenadesNextUpdateTime) {
					grenadesArray.Clear();
					grenadesArray = FindObjectsOfType<Grenade>().Where(s => isInScanZone(s.transform)).ToList();
					grenadesNextUpdateTime = Time.time + updateInterval * updateGrenadesIntervalMul;
				}
			
				if (isShowLoot && Time.time >= lootNextUpdateTime) {
					lootArray.Clear();
					lootArray = FindObjectsOfType<LootItem>().Where(s => (isInScanZone(s.transform) && LootFilter(s))).ToList();
					lootNextUpdateTime = Time.time + updateInterval * updateLootIntervalMul;
				}
			
				if (isShowLootBoxes && Time.time >= lootNextUpdateTime) {
					lootBoxesArray.Clear();
					lootBoxesArray = FindObjectsOfType<LootableContainer>().Where(s => (isInScanZone(s.transform) && LootBoxFilter(s))).ToList();
					lootNextUpdateTime = Time.time + updateInterval * updateLootIntervalMul;
				}
			
				if (isShowExits && Time.time >= exitsNextUpdateTime) {
					exitsArray.Clear();
					exitsArray = FindObjectsOfType<ExfiltrationPoint>().Where(ExitsFilter).ToList();
					exitsNextUpdateTime = Time.time + updateInterval * updateExitsIntervalMul;
				}
			} catch {
			}

			try {
				if (isShowPlayers) {
					showPlayersM();
				} else {
					if (playersArray.Count > 0)
						playersArray.Clear();
				}
					
				if (isShowGrenades) {
					showGrenadesM();
				} else {
					if (grenadesArray.Count > 0)
						grenadesArray.Clear();
				}
			
				if (isShowLoot) {
					showLootM();
				} else {
					if (lootArray.Count > 0)
						lootArray.Clear();
				}
			
				if (isShowLootBoxes) {
					showLootBoxesM();
				} else {
					if (lootBoxesArray.Count > 0)
						lootBoxesArray.Clear();
				}
			
				if (isShowExits) {
					showExitsM();
				} else {
					if (exitsArray.Count > 0)
						exitsArray.Clear();
				}
			} catch {
			}
		}

		void menuSelfShowM()
		{
			GUI.color = Color.white;
			GUI.Box(new Rect(0, 0, 400, 60), "Stats: " + playersCount + " " + texturesPreperaed);
			
			if (GUI.Button(new Rect(20, 20, 80, 30), "ESP")) {
				menuTabESP = !menuTabESP;
				menuTabLootFilter = false;
			}
			if (GUI.Button(new Rect(120, 20, 80, 30), "Filters")) {
				menuTabLootFilter = !menuTabLootFilter;
				menuTabESP = false;
			}
			
			if (menuTabESP) {
				GUI.Box(new Rect(0, 60, 400, 200), "ESP");
				isShowPlayers = GUI.Toggle(new Rect(20, 80, 100, 30), isShowPlayers, "Players");
				isShowCorpses = GUI.Toggle(new Rect(20, 100, 100, 30), isShowCorpses, "Corpses");
				isShowBones = GUI.Toggle(new Rect(20, 120, 100, 30), isShowBones, "Bones");
				isShowGrenades = GUI.Toggle(new Rect(20, 140, 100, 30), isShowGrenades, "Grenades");
				isShowLoot = GUI.Toggle(new Rect(20, 160, 100, 30), isShowLoot, "Loot");
				isShowLootBoxes = GUI.Toggle(new Rect(20, 180, 100, 30), isShowLootBoxes, "Loot Containers");
				isShowExits = GUI.Toggle(new Rect(20, 200, 100, 30), isShowExits, "Exits");
			}
			if (menuTabLootFilter) {
				GUI.Box(new Rect(0, 60, 400, 200), "Filter");
				isShowLootMisc = GUI.Toggle(new Rect(20, 80, 100, 30), isShowLootMisc, "Misc");
				isShowLootMeds = GUI.Toggle(new Rect(20, 100, 100, 30), isShowLootMeds, "Meds");
				isShowLootGuns = GUI.Toggle(new Rect(20, 120, 100, 30), isShowLootGuns, "Guns");
				isShowLootAddons = GUI.Toggle(new Rect(20, 140, 100, 30), isShowLootAddons, "Addons");				
				isShowLootBoxGood = GUI.Toggle(new Rect(20, 160, 100, 30), isShowLootBoxGood, "Gunbox");
				
				isShowScavExits = GUI.Toggle(new Rect(20, 200, 100, 30), isShowScavExits, "Scav Exits");
			}
		}
		
		void showPlayersM()
		{
			foreach (Player player in playersArray) {				
				Vector3 playerBoundingVector = new Vector3(
					                               Camera.main.WorldToScreenPoint(player.Transform.position).x,
					                               Camera.main.WorldToScreenPoint(player.Transform.position).y,
					                               Camera.main.WorldToScreenPoint(player.Transform.position).z);

				float distanceToPlayer = GetDistanceP(player.Transform);
				
				if (!(playerBoundingVector.z > 0.01) || !isInDrawZoneP(player.Transform))
					continue;
								
				float boxXOffset = Camera.main.WorldToScreenPoint(player.Transform.position).x;
				float boxYOffset = Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y + 10f;
				float boxHeight = Math.Abs(Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y - Camera.main.WorldToScreenPoint(player.Transform.position).y) + 10f;
				float boxWidth = boxHeight * 0.65f;
				
				Vector3 playerHeadVector = new Vector3(
					                           Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).x,
					                           Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y,
					                           Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).z);
				
				if (player.HealthController.IsAlive) {
					string drawColor = GetPlayerColor(player.Side);

					GUI.color = GhUI.Colors[drawColor];
					GhUI.oDrawBox(boxXOffset - boxWidth / 2f, (float)Screen.height - boxYOffset, boxWidth, boxHeight, drawColor);
					GhUI.oDrawLine(new Vector2(playerHeadVector.x - 2f, (float)Screen.height - playerHeadVector.y), new Vector2(playerHeadVector.x + 2f, (float)Screen.height - playerHeadVector.y), drawColor);
					GhUI.oDrawLine(new Vector2(playerHeadVector.x, (float)Screen.height - playerHeadVector.y - 2f), new Vector2(playerHeadVector.x, (float)Screen.height - playerHeadVector.y + 2f), drawColor);

					string playerInfo = isBot(player) ? "BOT" : player.Profile.Info.Nickname + " " + player.Profile.Info.Level;
					
					float playerHealth = player.HealthController.SummaryHealth.CurrentValue / 435f * 100f;
					string playerText = ((int)playerHealth) + " " + playerInfo + " " + (int)distanceToPlayer;

					Vector2 playerTextVector = GUI.skin.GetStyle(playerText).CalcSize(new GUIContent(playerText));
					GUI.Label(new Rect(playerBoundingVector.x - playerTextVector.x / 2f, (float)Screen.height - boxYOffset - 20f, 300f, 50f), playerText);
					
					if (isShowBones)
						showBonesM(player);
					
				} else {
					
					if (!isShowCorpses)
						continue;
					
					GUI.color = GhUI.Colors["white"];
					GhUI.oDrawBox(boxXOffset - boxWidth / 2f, (float)Screen.height - boxYOffset, boxWidth, boxHeight, "white");

					string corpseInfo = isBot(player) ? "BOT" : player.Profile.Info.Nickname + " " + player.Profile.Info.Level;
					
					string corpseText = corpseInfo + " " + (int)distanceToPlayer;

					Vector2 playerTextVector = GUI.skin.GetStyle(corpseText).CalcSize(new GUIContent(corpseText));
					GUI.Label(new Rect(playerBoundingVector.x - playerTextVector.x / 2f, (float)Screen.height - boxYOffset - 20f, 300f, 50f), corpseText);
				}
			}
		}
		
		void showBonesM(Player player)
		{
			Vector3 playerBoundingVector = new Vector3(
				                               Camera.main.WorldToScreenPoint(player.Transform.position).x,
				                               Camera.main.WorldToScreenPoint(player.Transform.position).y,
				                               Camera.main.WorldToScreenPoint(player.Transform.position).z);
			
			Vector3 playerNeckVector = new Vector3(
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Neck.position).x,
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Neck.position).y,
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Neck.position).z);
			Vector3 playerWeaponRoot = new Vector3(
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).x,
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).y,
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).z
			                           );
			Vector3 playerRFoot = new Vector3(
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.KickingFoot.position).x,
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.KickingFoot.position).y,
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.KickingFoot.position).z
			                      );
			Vector3 playerLPalm = new Vector3(
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.LeftPalm.position).x,
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.LeftPalm.position).y,
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.LeftPalm.position).z
			                      );
			Vector3 playerRPalm = new Vector3(
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.RightPalm.position).x,
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.RightPalm.position).y,
				                      Camera.main.WorldToScreenPoint(player.PlayerBones.RightPalm.position).z
			                      );
			Vector3 playerRThigh = new Vector3(
				                       Camera.main.WorldToScreenPoint(player.PlayerBones.RightThigh2.position).x,
				                       Camera.main.WorldToScreenPoint(player.PlayerBones.RightThigh2.position).y,
				                       Camera.main.WorldToScreenPoint(player.PlayerBones.RightThigh2.position).z);
			Vector3 playerLThigh = new Vector3(
				                       Camera.main.WorldToScreenPoint(player.PlayerBones.LeftThigh2.position).x,
				                       Camera.main.WorldToScreenPoint(player.PlayerBones.LeftThigh2.position).y,
				                       Camera.main.WorldToScreenPoint(player.PlayerBones.LeftThigh2.position).z
			                       );
			Vector3 playerCenterMass = new Vector3(
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Ribcage.position).x,
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Ribcage.position).y,
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Ribcage.position).z
			                           );
			Vector3 playerHeadVector = new Vector3(
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).x,
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y,
				                           Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).z);
			Vector3 playerLShoulder = new Vector3(
				                          Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).x,
				                          Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).y,
				                          Camera.main.WorldToScreenPoint(player.PlayerBones.LeftShoulder.position).z);
			Vector3 playerRShoulder = new Vector3(
				                          Camera.main.WorldToScreenPoint(player.PlayerBones.RightShoulder.position).x,
				                          Camera.main.WorldToScreenPoint(player.PlayerBones.RightShoulder.position).y,
				                          Camera.main.WorldToScreenPoint(player.PlayerBones.RightShoulder.position).z);

			string playerColor = GetPlayerColor(player.Side);
			string espColor = player.Profile.Health.IsAlive ? playerColor : "white";
			GUI.color = GhUI.Colors[espColor];
                    
			GhUI.oDrawLine(new Vector2(playerHeadVector.x, (float)Screen.height - playerHeadVector.y - 2f), new Vector2(playerCenterMass.x, (float)Screen.height - playerCenterMass.y), espColor);
                   
			GhUI.oDrawLine(new Vector2(playerLPalm.x, (float)Screen.height - playerLPalm.y - 2f), new Vector2(playerLShoulder.x, (float)Screen.height - playerLPalm.y + 2f), espColor);
			GhUI.oDrawLine(new Vector2(playerRPalm.x, (float)Screen.height - playerRPalm.y - 2f), new Vector2(playerRShoulder.x, (float)Screen.height - playerRShoulder.y + 2f), espColor);
			GhUI.oDrawLine(new Vector2(playerLShoulder.x, (float)Screen.height - playerLShoulder.y - 2f), new Vector2(playerRShoulder.x, (float)Screen.height - playerRShoulder.y + 2f), espColor);
			GhUI.oDrawLine(new Vector2(playerRThigh.x, (float)Screen.height - playerRThigh.y - 6f), new Vector2(playerCenterMass.x, (float)Screen.height - playerCenterMass.y + 30f), espColor);
			GhUI.oDrawLine(new Vector2(playerLThigh.x, (float)Screen.height - playerLThigh.y - 6f), new Vector2(playerCenterMass.x, (float)Screen.height - playerCenterMass.y + 30f), espColor);
			
		}
		
		void showGrenadesM()
		{
			foreach (Grenade grenade in grenadesArray) {				
				float distance = GetDistance(grenade.transform);
				Vector3 vector = new Vector3(Camera.main.WorldToScreenPoint(grenade.transform.position).x, Camera.main.WorldToScreenPoint(grenade.transform.position).y, Camera.main.WorldToScreenPoint(grenade.transform.position).z);
				
				if (!(vector.z > 0.01) || (distance > (drawDistance / 8)))
					continue;
				
				GUI.color = Color.white;
				GUI.Label(new Rect(vector.x - 50f, (float)Screen.height - vector.y, 100f, 50f), string.Format("{0}", grenade.name));
			}
		}
		
		void showLootM()
		{
			foreach (LootItem lootItem in lootArray) {
				Vector3 vector = new Vector3(Camera.main.WorldToScreenPoint(lootItem.transform.position).x, Camera.main.WorldToScreenPoint(lootItem.transform.position).y, Camera.main.WorldToScreenPoint(lootItem.transform.position).z);
				
				if (!(vector.z > 0.01) || !isInDrawZone(lootItem.transform))
					continue;
				
				string text = lootItem.name + " " + (int)GetDistance(lootItem.transform);
				
				GUI.color = Color.cyan;
				GUI.Label(new Rect(vector.x - 50f, (float)Screen.height - vector.y, 100f, 50f), string.Format("{0}", text));
			}
		}
		
		void showLootBoxesM()
		{
			foreach (LootableContainer container in lootBoxesArray) {
				Vector3 vector = new Vector3(Camera.main.WorldToScreenPoint(container.transform.position).x, Camera.main.WorldToScreenPoint(container.transform.position).y, Camera.main.WorldToScreenPoint(container.transform.position).z);
				
				if (!(vector.z > 0.01) || !isInDrawZone(container.transform))
					continue;
				
				string text = container.name + " " + (int)GetDistance(container.transform);
				
				GUI.color = Color.cyan;
				GUI.Label(new Rect(vector.x - 50f, (float)Screen.height - vector.y, 100f, 50f), string.Format("{0}", text));
			}
		}
		
		void showExitsM()
		{
			foreach (ExfiltrationPoint point in exitsArray) {
				Vector3 exfilContainerBoundingVector = new Vector3(
					                                       Camera.main.WorldToScreenPoint(point.transform.position).x,
					                                       Camera.main.WorldToScreenPoint(point.transform.position).y,
					                                       Camera.main.WorldToScreenPoint(point.transform.position).z);

				if (!(exfilContainerBoundingVector.z > 0.01))
					continue;
					
				string exitName = point.name;
					
				if (!isShowScavExits && exitName.Contains("scav"))
					continue;
					
				GUI.color = Color.green;
				GUI.Label(new Rect(exfilContainerBoundingVector.x - 50f, (float)Screen.height - exfilContainerBoundingVector.y, 100f, 50f), exitName);
			}
		}

		bool isBot(Player player)
		{
			return (player.Profile.Info.RegistrationDate <= 0);
		}

		string GetPlayerColor(EPlayerSide side)
		{
			switch (side) {
				case EPlayerSide.Bear:
					return "red";
				case EPlayerSide.Usec:
					return "blue";
				case EPlayerSide.Savage:
					return "yellow";
				default:
					return "yellow";
			}
		}
		
		float GetDistance(Transform transform_)
		{
			try {
				return Vector3.Distance(Camera.main.transform.position, transform_.position);
			} catch {
				return scanDistance + 50f;
			}
		}
		
		float GetDistanceP(BifacialTransform transform_)
		{
			try {
				return Vector3.Distance(Camera.main.transform.position, transform_.position);
			} catch {
				return scanDistance + 50f;
			}
		}
		
		bool isInDrawZone(Transform transform_)
		{
			try {
				return (GetDistance(transform_) <= drawDistance);
			} catch {
				return false;
			}
		}
		
		bool isInDrawZoneP(BifacialTransform transform_)
		{	
			try {
				return (GetDistanceP(transform_) <= drawDistance);
			} catch {
				return false;
			}
		}
		
		bool isInScanZone(Transform transform_)
		{
			try {
				return (GetDistance(transform_) <= scanDistance);
			} catch {
				return false;
			}
		}
		
		bool isInScanZoneP(BifacialTransform transform_)
		{	
			try {
				return (GetDistanceP(transform_) <= scanDistance);
			} catch {
				return false;
			}
		}
		
		bool PlayerFilter(Player player_)
		{
			try {
				return (player_.Transform != null);
			} catch {
				return false;
			}			
		}
		
		bool LootFilter(LootItem lootItem)
		{		
			try {
				if (lootItem == null || lootItem.name == null || lootItem.name == string.Empty)
					return false;
			
				List<string> filter = new List<string>();
			
				if (isShowLootMisc)
					filter.AddRange(lootFilterMisc);
				if (isShowLootMeds)
					filter.AddRange(lootFilterMeds);
				if (isShowLootGuns)
					filter.AddRange(lootFilterGuns);
				if (isShowLootAddons)
					filter.AddRange(lootFilterAddons);
			
				if (filter.Count > 0) {
					foreach (string fltr in filter) {
						if (lootItem.name.ToLower().Contains(fltr)) {
							filter.Clear();
							return true;
						}
					}
					filter.Clear();
					return false;
				}
				filter.Clear();
				return true;
			} catch {
				return false;
			}
		}
		
		bool LootBoxFilter(LootableContainer lootbox)
		{
			try {
				if (lootbox == null || lootbox.name == null || lootbox.name == string.Empty)
					return false;
			
				List<string> filter = new List<string>();
			
				if (isShowLootBoxGood)
					filter.AddRange(lootBoxFilterGood);
			
				if (filter.Count > 0) {
					foreach (string fltr in filter) {
						if (lootbox.name.ToLower().Contains(fltr)) {
							filter.Clear();
							return true;
						}
					}
					filter.Clear();
					return false;
				}
				filter.Clear();
				return true;
			} catch {
				return false;
			}
		}
		
		bool ExitsFilter(ExfiltrationPoint exit_)
		{
			try {
				return exit_ != null;
			} catch {
				return false;
			}
		}
	}
}
