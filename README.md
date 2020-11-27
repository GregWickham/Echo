# Echo - A bridge from CoreNLP to SimpleNLG

Echo does two things:

1.  Accept output from the Stanford CoreNLP annotation pipeline, and transform it into a specification that can be realized by SimpleNLG; and
2.  Provide a user interface that can inspect the transformed specification as a directed graph.

Echo connects to both CoreNLP and SimpleNLG in their server forms.  This permits relatively loose coupling to both systems, and allows them to be upgraded to new versions without changing the .NET client code (unless the server API's change).

To use Echo, the first step is to install and run the servers for CoreNLP and SimpleNLG.  The folder called `Server BAT files` contains two Windows batch files that I use to start the servers.  These batch files refer to specific locations in my file system, so you'll need to modify them to reflect the actual locations where you install the `.jar` files.  The SimpleNLG server requires two `.jar` files that are not included in the SimpleNLG distribution -- `hsqldb_6148.jar` and `lexAccess2013dist.jar`.

Once the servers are running, you're ready to run Echo.  You can test the SimpleNLG connection by using Visual Studio Test Explorer to run the unit tests in the `SimpleNLG_Tests` project.  The unit tests in `ParseAndRealize_Tests` exercise both the CoreNLP **and** SimpleNLG ends of the bridge.  Sometimes a few of the tests in `ParseAndRealize_Tests` will fail for reasons that I don't understand.  If this happens, try running those tests individually and they should work.

The Visual Studio startup project for the user interface is `Echo_UI_WPF`:

![Image of Echo WPF Window](/docs/images/EmptyWindow.jpg)

The menu bar contains drop-down items to set the hostname and port for the CoreNLP and SimpleNLG servers.  The default values are the same as the defaults distributed with each server -- *i.e.* `localhost:9000` for CoreNLP and `localhost:50007` for SimpleNLG.

To run the Echo pipeline, enter some text in the upper text box and press the "Parse" button:

![Image of parsed and realized sentence](/docs/images/ParsedAndRealizedRootSelected.jpg)

This causes the following to happen:

1.  The entered text is sent to the CoreNLP server and annotated.  
2.  Echo uses selected information from the Part-Of-Speech, Contituency, and Enhanced++ Dependency annotations to construct the object graph displayed in the window.
3.  The object graph constructs an XML specification and sends it to the SimpleNLG server to be realized.
4.  The realized text is displayed in the text box at the bottom of the window.

In the graph, the lowest layer of nodes represent parse tokens.  These nodes are different from the other nodes, and this difference is reflected in their visual style.  The other nodes represent the syntactic structure of the parsed text, and their labels correspond roughly to the specification for [Penn Treebank II Constituent Tags](http://www.surdeanu.info/mihai/teaching/ista555-fall13/readings/PennTreebankConstituents.html).


When the parse / realize process is done, the root node of the graph is initially selected.  This is shown in the above figure by the red border around the uppermost node.
