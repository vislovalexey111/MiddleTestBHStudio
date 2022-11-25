# MiddleTestBHStudio
Test project for a Middle Unity Developer from BH Studio.
Made by Vislov Alexey.

The task:
1. Create 3D scene that has a plane with static primitives, Spawn Points and directional light to simulate the game location.
2. With Mirror Networking package, create Room System: players must connect to the room before starting the game.
3. The Game must start in fullscreen with cursor always centered (not shown). All players must spawn in random spawn points.
4. Each player has controls: Movement (WASD), Dash (Left Mouse Button) and Third Person Camera (rotated by mouse).
5. If Player A is dashing and hits Player B, Player B recieves damage (changing color) and Player A gets a point.
6. While recieving damage, the player is invincible.
7. If Player A gets enough points, show the winner name and start countdonw. If countdown ends - restart the match.
8. Each player must have editable parameters: dash distance, damage duration, points count, player name and countdown seconds.