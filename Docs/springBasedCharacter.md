# Spring Based Character Controller
**Concept:** *Floating Character Controller with Spring Forces* 

A Capsule Character Controller floating due to a *Spring Force* acting as a snapping and repelling Force to the ground.

This behavior makes it possible to go through any type of terrain because we are not actually colliding with it and we also avoid dealing with external forces like drag. 

---

<img src="Img/Spring-Character-Controller-Thumbnail.PNG" width="700" alt="spring">

---

**Physics:** A Spring Force and a Damping Force are applied between the Character and the Floor so that we can float at a desired distance with some tunable parameters (spring force, damping force, etc) which helps controlling how the character responds to inputs, terrain and external Forces. Also the behavior for how the Characters stays Upright and how the movement are Controlled similarly by a Spring and Damping Systems applying torque and linear Forces respectively.

Work In Progress: Document everything with its corresponding methods on code (movementbehaviour.cs, velocitychange.cs, states, etc etc)
