# CoinWars
The game

Demo Videos:<br />
https://www.youtube.com/watch?v=qOuIEyAAFgM<br />
https://www.youtube.com/watch?v=Lzvdf_0ps0s<br />
(many many more. can trace history of this game by watching the vids my YT account!)


Ok likely many unnecessary/stale files in here...here's some navigation in Unity3D--Checked with Unity5<br />
1) Latest game scene: Assets>testConfigs<br />
2) reflex AI scene: Assets>AI>AI

**In Depth Rules/Gameplay:**
tldr version: 2 players control 2 fleets of red or yellow collored coins and have the coins fight each other untill all coins of one color have "died".

How to kill a coin:
There are two ways to kill a coin:<br />
1) Note that each coin has an energy bar (the halo around it). As coins incur damage (inflicted collisions by other coins and especially so by said coins equipped by the Dynamite powerup!) this bar disappears.  Once the halo is gone, the coin dies.<br />
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Healthy.png" alt="orignal"> 
<br>
<img src="https://dl.dropboxusercontent.com/u/105935968/CoinWars/Snapshots/Damaged.png" alt="orignal"> 
<br>
2) Knock the coin off the platform. It will die.



Features:
Currency Exchange: Merge and split coins by "D" key
Powerups: Dynamite, Fireball, Mirror,.. Each has interesting effects when indicent coin picks it up
TerrianSquares: Expand your territory--connected components can be used as portals (using "S" key) <-migrate this to "Gameplay"
