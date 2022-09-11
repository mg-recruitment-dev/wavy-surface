# Wavy Surface

## Answers to questions
Here are the answers to questions asked in the final section of this challenge:
1. Increasing the mesh size causes the frame rate to drop gradually. I would say, that the animation starts to get choppy at around 500k tiles ( 1M triangles ). That's of course a rough estimate and it also depends on conditions under which test is performed. For example, I tested the performance in the Unity Editor ( where it is usually worse than in build ) and on my machine ( which can be better or worse than some other machine ).
2. First of all, setting the same color for each vertex could be replaced by using for example a color property in a shader. We could also reduce the amount of data by, for example, increasing the size of a single tile. We can also animate the mesh less frequently, so the vertex data won't change that often. Unity also offers mechanisms for compressing mesh data which we could use.
3. This wavy surface could be, for example, animated using vertex shader instead of manipulating vertex data in a script. As I said earlier, we could also use color property in a shader instead of setting vertex colors.
4. The frame rate we compute ourselves is closer to what we actually get inside the editor. Rendering the editor window in which our app / game is running, adds some overhead on its own and editor's stats window tries to compensate for that. It only shows, what time it takes to render our app while in reality the app runs slower because of computations concerning the editor window.
5. To improve the quality of the rendered image we could, for example, use some post-processing, improve the shading on our plane, add some textures to it or try working with different lights and their values. It all depends on what kind of effect we want to achieve.
