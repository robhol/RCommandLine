# RCommandLine

RCommandLine is designed to be as simple and succinct as possible from the user's perspective.

RCommandLine allows you to create simple POCOs exposing properties for commands, flags and arguments that can be specified through the command line. These properties are adorned with Attributes in order to specify desired functions.

## Features

### Minimal configuration

RCommandLine is designed to intuit as much information as possible. While some information is required, most of it's extrapolated and can be overridden if you want manual control.

### Simplistic association

Somewhat contrary to tradition, flag values will be read in the order the flags themselves are given by the user. Any amount of "bundled" short flags is supported.

### Command handling (optional)

In addition to handling parameters to your programs, RCommandLine will also simplify command handling. Commands are configured as class-level Attributes and point to other classes, supporting as many levels as you'd like. Commands will be read from the beginning of the input string for as long as possible.

##Example

Given flags defined by the short names 'a', 'b', and 2 required arguments, the following input strings are all equivalent:

```
MyProgram -a aValue -b bValue argument1 argument2 
MyProgram argument1 argument2 -ab aValue bValue
MyProgram argument1 argument2 -ba bValue aValue
MyProgram -b bValue argument1 -a aValue argument2 
    ... etc
```

###Better example







## Testing

Basic tests (MSTest) are included in the TestRCommandLine project.

## License