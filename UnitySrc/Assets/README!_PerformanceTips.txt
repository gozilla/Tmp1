Hello my dear friends,


Realtime is very sensisitve to tuning. For the best overall performance, some tweaking is always needed.

In the specific case of the Etherea1 extension, I would like to make some quick suggestions to improve performance:

-- Size Split and Size Rejoin

These parameters highly affect performance. They are the LOD adjustments, the distances for which the planet surface splits into more details (Size Split) or discards details (Size Rejoin).

Imagine a planet from a very far distance; the planet will be just 6 patches (front, back, left, right, top and bottom patches). It does not have great details, but as we are very far from the planet, we don’t really need the details.

Now imagine we start approaching that planet; at some point, the patch of the planet where the camera is closer will need more detail, so it’ll need to Split into 4 new patches and create more detail for that region. What is the best distance for the planet to split? That distance is given by the parameter Size Split (or, how many patch sizes of distance it should split).

If we put Size Split a high value, lets say 10, then when we are at 10 times the size of one of a patch (lets say, the front patch), that patch will split into 4 front patches and create more details there. This means that still from a long distance the patch will already split, and also means that more and more patches will split even when they are distant from us, giving less LOD popping BUT as we’ll have more and more splits, we’ll have more and more geometry and of course less performance.

If we put Size Split a low value, lets say 2, then only when we are at 2 times the size of a patch, in other words, very close to that patch, it’ll split and create more details. So, other patches which will be at 3 or more won’t split and won’t generate more details. This will cause more LOD popping as they will split close to us, BUT will give us far less geometry and much more performance!

Size Rejoin on the other side, is the inverse. It tells Etherea1 when patches should be discarded, releasing geometry, details, memory and... performance. It MUST be greater than Size Split, and I recommend 2x (or more) the value chosen for Size Split.

Some good pair values for Size Split and Size Rejoin:

For Performance (but more LOD popping):

    Size Split = 2
    Size Rejoin = 4

For less LOD popping (and of course less performance):

    Size Split = 7
    Size Rejoin = 10 (or more)

-- Normal Quality

This parameter also greatly affects performance. This is the terrain normal mapping resolution for each terrain patch. A higher resolution normal will give more illusion of details, costing more GPU cycles. We don’t always need maximum normals quality, so I suggest you to start with Standard normals and fine tune the other parameters like noise layers frequency, height, etc. If you have a really nice performance after tuning everything up, you can then try raising a bit the resolution and see how it affects the overall performance.

-- Patch Quality

This parameter algo greatly affects performance, although far less than the others. This is the resolution of each terrain patch (the grid of vertices) generated for a planet. A higher resolution patch will give more terrain mesh details, costing a bit more of CPU cycles. Try experimenting lower resolutions first, and when you have all other parameters fine tuned and with a nice performance, you may consider raising this one a bit.

-- Noise Layers and Noise Octaves

The more noise layers you add to a given planet, the more processing time is used to create its terrain, but more variation and possibly more interesting terrains you get. I like using one or two layers at maximum for my experiments, but it depends a lot on your application, target hardware and preferences, of course.

Each noise layer works by doing a number of noise passes (Octaves) generating random heights. The more octaves you set for a noise layer, the more height randomness you have and more processing time is used. On my own experience, I found that 6 to 12 octaves are sufficient to generate quite interesting terrains.

Please note that all noise parameters will affect randomness of terrain. I like to recommend reading of this page ( http://freespace.virgin.net/hugo.elias/models/m_perlin.htm ) to better understand how noise works. Please read it and you’ll surely better understand how to tune the noise generators to create terrain for your planets. You don’t need to understand that sample code, just the initial text. I used that article as a reference just because I think his explanation is nice, short and clear. Etherea1 code is not based on that article at all, but it’s great and fits, nevertheless.


Cheers,

Vander