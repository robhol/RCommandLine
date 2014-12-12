# RCommandLine

RCommandLine is designed to be as simple and succinct as possible from the user's perspective.

RCommandLine allows you to create simple POCOs exposing properties for commands, flags and arguments that can be specified through the command line. These properties are adorned with Attributes in order to specify desired functions.

## Features
### Minimal configuration

RCommandLine is designed to intuit as much information as possible. All you have to do is make any class with the desired properties, tell RCommandLine what they should contain, and it handles the rest.

Hopefully, the defaults should all be sane, but if you require more control for whatever reason, it's possible to customize a lot of the process.

### Standard syntax

RCommand is - with a notable exception - generally faithful to standard syntax from both Unix and Windows worlds, and supports both syntax forms out of the box. You can also customize the syntax to a large degree.

Somewhat contrary to tradition, flag values will be read in the order the flags themselves are given by the user. Any amount of "bundled" short flags is supported.

#### Universal by default
Default settings accept both Unix and Windows-style syntax.

#### Standard Unix-style settings
<pre>
# Long flag names:
MyProgram  --a-value 3   --b-value 4  # Standard syntax
MyProgram  --a-value --b-value   3 4  # Allowed and equivalent - flags do not need to be followed immediately by their value, but are picked up in the same order

# Short flag names:
MyProgram  -a 3 -b 4    # Standard syntax
MyProgram  -abx 3 4     # Bundled syntax (assuming x takes no value, -xab and -axb are equivalent)
</pre>

#### Standard Windows-style settings
<pre>
# Long flag names:
MyProgram  /a-value 3   /b-value 4  # "Standard" syntax
MyProgram  /a-value /b-value   3 4  # Allowed - flags do not need to be followed immediately by their value
</pre>

Using short flag names, the Windows preset also permits direct flag assignment using : and = as seen below.
<pre>
# Short flag names:
MyProgram  /a:3 /b=4 /c 5 # "Standard" syntax with direct assignment of a and b
MyProgram  /ab 3 4        # Note: will try to find by long name first, then try to assign as "bundle"
</pre>

### Command handling (optional)

In addition to handling parameters to your programs, RCommandLine will also simplify command handling. This allows you to easily define commands resembling those of `git` and similar tools:

<pre>
<b>MyProgram file read</b>   myfile.txt --format ASCII
               <b>write</b>  file2.dat

<b>MyProgram user add</b>    Alice
               <b>delete</b> Bob
</pre>

Commands can be defined in as many "layers" as you want, are tied to separate Options classes (which may be unrelated or organized in an inheritance structure). 

### Details

For more details, refer to the wiki on https://github.com/robhol/RCommandLine/wiki - although the primary authority is the actual code and tests.

### Testing

Basic tests (MSTest) are included in the TestRCommandLine project.

### License

In plain English: 
* Feel free to use it for any project, redistributing or embedding as needed
* If you use it for anything significant:
    * An honorable mention would be appreciated. :)
    * ... as would an e-mail describing what you're using it for, because I'm curious.
* Please link back to either http://robhol.net/RCommandLine (preferred) or https://github.com/robhol/RCommandLine
* Please don't sell on its own, and don't pass off as your own.  
This also applies to forked versions and derivatives that don't drastically alter functionality.
* If you fork and modify RCommandLine with bugfixes or features, I'd love to see pull requests.