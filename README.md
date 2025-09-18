# Unity-Physics-Playground
Unity Physics-based motion project. Experiments showcasing gameplay mechanics

![Unity CI](https://github.com/marc-gomez-tejedor/Unity-Physics-Playground/actions/workflows/Build.yml/badge.svg) ![License](https://img.shields.io/badge/license-MIT-blue)

---

## About the project
A ~1 000-hour -work in progress- solo project focused on physics gameplay in Unity
Covering:

| Phase | Hours | Key deliverables | Physic Principle |
|-------|-------|------------------|------------------|
| **0 · Setup** | ~10 h | Monorepo, CI | … |
| **1 · Orbits Scene** | ~40 h | Solar system with: a Central Star, a few Planets and a Satellite | Gravity-Based Orbits |
| **2 · Spaceship Scene** | ~50 h | 2001: A Space Odyssey's inspired Space Station V | Centripetal Force simulating Artificial Gravity |
| **3 · Floating Character Controller** | ~100 h | Floating Capsule based on tunable Spring-like motions | Spring Forces: Floating, Moving and Straightening |
| **4 · Rolling Character Controller** | ~60 h | Following Catlikecoding's Movement Tutorial | Rigidbody movement of a Sphere |
| **5 · Procedural Animation** | ~XX h | Using FABRIKS to animate the Character | Inverse Kinematics for Robot-like Limbs |

---

## ShowCases

| Name | Summary | Demo IMG |
|------|---------|----------|
| **Orbits** | *Scene resembling a Solar System* Every Object applies Gravity to eachother based on *Newton's Gravitational Law* with a custom *G* constant in order to make it more dynamic. More detailed explanation on: /docs/orbits.md | <img src="Docs/Img/Orbits-Thumbnail.PNG" width="420" alt="Orbits"> |
| **SpaceShip** | *2001: A Space Odyssey's inspired Space Station V* A cirular shaped Space Station rotating around its central axis in order to *simulate Artificial Gravity via the imaginary Centrifugal Force*. More detailed explanation on: /docs/spaceship.md | <img src="Docs/Img/SpaceShip-Thumbnail.PNG" width="420" alt="SpaceShip"> |
| **Spring-based Character** | *Floating Character Controller with Spring Forces* A Capsule Character Controller floating due to a *Spring Force* acting as a snapping and repelling Force to the ground, this behavior makes it possible to go through any type of terrain because we are not actually colliding with it and we also avoid dealing with external forces like drag. A Spring and a Damping Force are applied between the Character and the Floor so that we can float at a desired distance with some tunable parameters (spring force, damping force, etc) which helps controlling how the character responds to inputs, terrain and external Forces. Also the behavior for how the Characters stays Upright and how the movement are Controlled similarly by a Spring and Damping Systems applying torque and linear Forces respectively. More detailed explanation on: /docs/springbasedCharacter.md | <img src="Docs/Img/Spring-Character-Controller-Thumbnail.PNG" width="420" alt="Spring-based Character"> |
| **Rolling Character** | *Following Catlikecoding.com tutorial on Movement* The tutorial mainly focuses on a Third Person Controller that follows a Sphere with drag coefficient of 0, it shows some examples on how to implement some behaviors like sliding, rolling, jumping, climbing, swimming, etc. I am currently working on taking some ideas from this Tutorial and applying them to my current Spring-Based Character controller. More detailed explanation on: /docs/rollingCharacter.md | <img src="Docs/Img/Rolling-Character-Controller-Thumbnail.PNG" width="420" alt="Rolling Character"> |

---

## Reproducible environment

| Unity LTS + Web Module |
|------------------------|
| <img src="Docs/Img/Unity-6.0-LTS-install.PNG" width="420" alt="Unity 6 LTS + WebGL"> |

---

## Cloning

```bash
git clone git@github.com:marc-gomez-tejedor/Unity-Physics-Playground.git
# Unity Hub → Add project → select repo root
# Confirm it opens with Unity 6.0 LTS
```
