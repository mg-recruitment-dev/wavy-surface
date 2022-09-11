# Wavy Surface

## Table of contents
* [Installation](#installation)
* [Notes and thoughts](#notes-and-thoughts)
    * [Lighting Shader](#lighting-shader)
    * [Builds](#builds)
    * [Bug in WebGL Build](#bug-in-webgl-build)
* [Answers to questions](#answers-to-questions)

## Installation
Project uses Git LFS for storing bigger files. [Here](https://docs.github.com/en/repositories/working-with-files/managing-large-files/installing-git-large-file-storage) you can find instructions on how to install it. Solution was developed using Unity version 2021.3.9f1, but it should also work on other modern Unity versions.

## Notes and thoughts

### Lighting Shader
I assumed that the shader used for lighting the surface was supposed to be fragment / vertex shader, but we could also implement lighting using, for example, surface shader's lighting function. Here I also implemented code for applying colors based on the vertex color data. It wasn't said explicitly, but I thought that these values were meant to be used somewhere.

### Builds
I uploaded builds produced during this challenge in case they were needed. These builds are in .zip files under BuildArchives directory.

### Bug in WebGL Build
Here is what my thought process behind fixing WebGL bug looked like: I noticed that the bug is caused by WebGL's lack of support for geometry shaders. After that I started to examine the code of the wireframe shader and tried to come up with a way to implement this effect using only vertex and fragment functions. I thought that I could skip the geometry shader step, if I prepopulate mesh with the data that is calculated inside the geometry shader. That's why, in my script, I saved barycentric coordinates as vertex colors. Then I created an alternate version of wireframe shader using only vertex and fragment functions. And finally, I added my newly created shader as a fallback to the original wireframe shader, which fixed the bug.

## Answers to questions
Here are the answers to questions asked in the final section of this challenge:
1. Increasing the mesh size causes the frame rate to drop gradually. I would say, that the animation starts to get choppy at around 500k tiles ( 1M triangles ). That's of course a rough estimate and it also depends on conditions under which test is performed. For example, I tested the performance in the Unity Editor ( where it is usually worse than in build ) and on my machine ( which can be better or worse than some other machine ).
2. First of all, setting the same color for each vertex could be replaced by using for example a color property in a shader. We could also reduce the amount of data by, for example, increasing the size of a single tile. We can also animate the mesh less frequently, so the vertex data won't change that often. Unity also offers mechanisms for compressing mesh data which we could use.
3. This wavy surface could be, for example, animated using vertex shader instead of manipulating vertex data in a script. As I said earlier, we could also use color property in a shader instead of setting vertex colors.
4. The frame rate we compute ourselves is closer to what we actually get inside the editor. Rendering the editor window in which our app / game is running, adds some overhead on its own and editor's stats window tries to compensate for that. It only shows, what time it takes to render our app while in reality the app runs slower because of computations concerning the editor window.
5. To improve the quality of the rendered image we could, for example, use some post-processing, improve the shading on our plane, add some textures to it or try working with different lights and their values. It all depends on what kind of effect we want to achieve.
