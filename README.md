# FNF to Osu Mania Converter API
c# API for Funkin' JSON files that converts them to **4K** Osu mania file format, supports almost every engine as i know(Psych, Kade, legacy, V-slice don't tested). Sliders work too, yay.

I made it to make the job of creating fnf charts in osu a much easier that just recreating it from scratch.

The Api only has function to save an .osu file in the output of the given JSON.
For the cons due to incomplete see the TODO section.

# Usage
Well if you know coding much you know how to use it, I can only point my finger at the single method that you [need](https://github.com/Fadeyauso/FNFToOsuManiaConverter/blob/main/FNFToOsuManiaConverter/FNFChart.cs#L16)
So don't open any issues with the quotes like "bro that's cool ash but how to use it?" i'll just close them.
Also you can modify the api as you need, make forks n shit, pull requests, and whatever.

# Cons
2. **No .osz file creation** (So you need to do it yourself, or i''ll do it someday idk)
3. **If you choose both sides notes parsing notes will overlap each other** (so fix it or use only one side, added both just for fun)
4. **Notes overlapping each other in any sides idk why**
5. **No Audio files combining like inst and voices together in one file** (well for now do it yourself, maybe someday i'll do it)
6. **No .osu file customization like Title, AudioFilename, and other.** (Again, you can make a winForms app for example and add this functionality, but change the hardcoded structure in the code first)

# Libs used
[Json.NET](https://github.com/JamesNK/Newtonsoft.Json)