# sickle
Sickle is a mod for Scythe Digital Edition to improve the performance of the AI

# How to use (Steam)
1. Take a backup of file: C:\Program Files (x86)\Steam\steamapps\common\Scythe Digital Edition\Scythe_Data\Managed\Assembly-CSharp.dll and move it elsewhere
2. Download the Assembly-CSharp.dll from the main branch and save it to C:\Program Files (x86)\Steam\steamapps\common\Scythe Digital Edition\Scythe_Data\Managed
3. Run a game (be sure to use the "Hard" AI, I am not making changes to the Easy/Medium AI)

I don't know if this AI will work with multiplayer; if it does, then it is probably the host's version of the AI that runs. Or, it may crash the game. Submit an Issue if the game crashes.

# Bugs?
Submit Issues to the Issues tab. 

# Not Steam?
The file is *probably* the same, and goes to the same path ...\Scythe_Data\Managed\Assembly-CSharp.dll

# Mobile
Sorry, you're out of luck. Maybe the official developer will implement these changes into an official update! That would be welcome.

# File Descriptions
Assembly-CSharp.dll This is the game code, compiled for use.

# Cheat sheet
Upgrade: AiAction.cs look for: UpgradePriorityAdvanced  
Deploy: AiStrategicAnalysisAdv.cs: look for public override void UpdateMechOrder(AiPlayer aiPlayer)  
Recruit: AiStrategicAnalysis.cs: Look for public void UpdateRecruitOrder(AiPlayer aiPlayer)  
Build: AiStrategicAnalysis.cs: Look for public void UpdateBuildingOrder(AiPlayer aiPlayer)  

# AiAction.cs — “How the AI executes actions”
This file defines the core execution engine for AI actions.
It does not decide which action to take — it executes the action that the AI logic has already chosen.
Execute TOP actions (Move, Produce...)
Execute DOWN actions (Deploy, Recruit...)
Movement logic
Mech deployment logic (WHERE to deploy)
Upgrade priority logic

# AiStrategicAnalysis.cs — “How the AI thinks and plans”
This file is the brain of the AI.
It analyzes the board state and produces priorities that AiAction uses.
Major responsibilities
A. Run() — the master planning function
This is called every turn.
It updates:
- Encounter targets
- Factory distance
- Resource access
- Resource demand
- Trade loops
- Produce loops
- Whether the AI can upgrade/build/deploy/enlist
- Worker movement priorities
- Mech movement priorities
- Attack evaluation
- Building order
- Mech order
- Recruit order
This is the AI’s “strategic snapshot” of the world.

B. Resource logic
The AI computes:
- How many of each resource it can access
- How many it needs to finish all stars
- Which resource is highest priority
- Whether it should trade or produce
This drives almost every economic decision.

C. Worker movement logic
The AI identifies:
- “Useless” workers (not on valuable hexes)
- Where they should move
- Whether they should ride mechs
- Whether they should unload
This is the engine behind worker repositioning.

D. Mech movement logic
The AI computes:
- All reachable hexes
- Attack opportunities
- Resource opportunities
- Multi-step movement (including passengers)
- Special faction/mat rules (e.g., Saxony Industrial)
This is the tactical movement system.

E. Combat evaluation
isWorthAttacking() determines whether the AI should attack a hex.
It considers:
- Enemy units
- Combat cards
- Power
- Resources on the hex
- Faction perks
- Worker penalties
- Multi-unit support
This is the AI’s combat heuristic.

F. Encounter & Factory targeting
The AI tracks:
- Nearest encounter
- Distance to factory
- Whether the hex is safe
This influences movement and priorities.

# AiStrategicAnalysisAdv.cs — What this file does
This file defines the advanced strategic analysis module used on Hard difficulty.
It extends the basic analysis with much deeper planning, including objectives, multi‑stage movement logic, and faction/mat‑specific heuristics.
Core responsibilities
- Reads the AI’s objective cards and activates special behaviors (e.g., Balanced Workforce, Machine Over Muscle)
- Computes:
- Resource access
- Resource demand
- Resource priority (with many faction/mat overrides)
- Determines:
- Whether the AI can upgrade, deploy, build, or enlist
- Whether it must move to build (Saxony Industrial)
- Computes:
- Worker movement targets
- Mech movement targets
- Objective‑related movement
- Combat‑related movement
- Secondary movement
- Scatter behavior (spreading out units)
- Determines mech unlock order (with faction/mat overrides)
- Determines recruit one‑time bonus priority
- Identifies objective areas on the map and sets targets
In short
AiStrategicAnalysisAdv is the AI’s “strategic brain”: it evaluates the board, sets priorities, chooses movement targets, and plans how to reach objectives.


# AiPlayer.cs — What this file does
This file defines the AI player entity.
It is the central controller for an AI opponent’s turn, state, and decision execution.
Core responsibilities
- Stores references to:
- The Player object
- The GameManager
- The AI’s strategic analysis module
- The AI’s kickstart/opening logic
- Initializes all AI actions by scanning the player mat and mapping:
- Top actions → GainTypes
- Down actions → Upgrade/Mech/Building/Recruit positions
- Runs the AI’s turn:
- Chooses a section
- Executes top action
- Handles combat if triggered
- Executes bottom action
- Ends turn
- Handles:
- Combat logic (all stages)
- Using faction abilities in combat
- Withdrawing units after losing combat
- Paying for actions (including Coercion logic)
- Finding resources to pay costs
- Handling encounters and factory cards
- Moving workers for building placement
- Checking and completing objectives
In short
AiPlayer is the AI’s “brainstem”: it runs the turn, executes actions, handles combat, pays costs, and manages all in‑turn behavior.


# AiKickStart.cs — What this file does
AiKickStart defines the AI’s opening script for the first several turns of the game.
It is essentially a hard‑coded opening book, similar to an opening repertoire in chess.
Core purpose
For each faction + player mat combination, and for each early turn number, it:
• 	Chooses a specific Top Action (Produce, Move, AnyResource, Power, etc.)
• 	Optionally sets:
• 	 (what to trade for)
• 	 (a custom scripted movement sequence)
• 	 (where to deploy the first mech)
• 	Inserts the chosen action into the AI’s action options with maximum priority
()
What it contains
• 	A giant switch tree:
• 	First by Faction
• 	Then by PlayerMatType
• 	Then by TurnCount
Each combination has a small, fixed script for the first few turns.
What these scripts do
They typically:
• 	Produce resources on turn 0
• 	Trade for needed resources on turn 1
• 	Move workers into optimal early‑game positions
• 	Move the character toward encounters or key hexes
• 	Set up early mech deployment
• 	Ensure the AI has the resources to unlock its first mech or building
In short
AiKickStart is the AI’s “opening book,” giving it a strong, deterministic start before normal strategic logic takes over.

# AiKickStartAdv.cs — What this file does
AiKickStartAdv is the advanced version of the same concept, used on Hard difficulty.
It contains more sophisticated, more faction‑specific, and more map‑aware opening scripts.
Core purpose
Just like AiKickStart, it scripts the first several turns, but:
• 	With more complex movement
• 	With more conditional logic
• 	With more faction/mat‑specific optimizations
• 	With more aggressive or objective‑driven openings
What it contains
• 	A similar giant switch tree:
• 	Faction → PlayerMatType → TurnCount
• 	But with more branches, including:
• 	Togawa Industrial, Engineering, Mechanical, Agricultural, Militant, Innovative
• 	Albion Industrial, Patriot, Mech, Militant, Innovative
• 	Nordic Innovative
• 	Many more scripted openings than the basic version
What these scripts do
Compared to the basic version, these scripts:
• 	Move multiple workers in coordinated patterns
• 	Use tunnels more intelligently
• 	Position workers for multi‑hex production loops
• 	Set up early combat or encounter grabs
• 	Use mech passenger logic in the opening
• 	Position the character for early objectives
• 	Spread workers to maximize resource diversity
In short
AiKickStartAdv is the “expert opening book,” giving Hard‑difficulty AI a polished, optimized, faction‑specific early game.
