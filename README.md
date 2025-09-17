This repository contains a set of Unity C# scripts that control **player movement, collisions, level flow, object oscillation, explosions, enemy health, and quitting the game**.  

---

## 📂 Scripts Overview

### 🎮 Movement.cs
- Handles player/rocket movement with **thrust** and **rotation** using the Unity Input System.  
- Plays engine sound and particle effects while thrusting or rotating.  

### 💥 CollisionHandler.cs
- Detects collisions with objects in the scene.  
- If the player collides with a **friendly** object, nothing happens.  
- If the player collides with the **finish**, it triggers a success sequence and loads the next level.  
- Any other collision triggers a crash sequence and reloads the current level.  
- **Debug keys**:  
  - `L` → Skip to next level  
  - `C` → Toggle collision detection  

### 🔁 Oscillator.cs
- Moves an object back and forth between two points using `Mathf.PingPong`.  
- Creates smooth oscillating movement for platforms or obstacles.

### ⎋ QuitApplication.cs
- Quits the game when **Escape** is pressed.  
- Prints a debug message in the Console (only visible in the Unity Editor).  

---

## 🕹️ Game Flow
1. **Movement.cs** → Controls the player’s thrust and rotation.  
2. **CollisionHandler.cs** → Handles success, crash, or ignore outcomes on collision.  
5. **Oscillator.cs** → Animates moving obstacles/platforms.  
6. **QuitApplication.cs** → Allows quitting the game with Escape.  

---

## ▶️ How to Use
- Attach each script to the appropriate GameObject in Unity.  
- Assign references (Audio, Particle Systems, etc.) in the **Inspector**.  
- Press **Play** in Unity and test movement, collisions, enemies, and quitting functionality.  
