# Echo - A bridge from CoreNLP to SimpleNLG

Echo does two things:

1.  Take the output of the Stanford CoreNLP annotation pipeline, and transform it into a specification that can be realized by SimpleNLG; and
2.  Provide a user interface that can inspect the transformed specification as a directed graph.

Echo connects to both CoreNLP and SimpleNLG in their server forms.  This permits looser coupling to both systems, and allows them to be upgraded to new versions without changing the .NET client code (unless the server interfaces change).

To use Echo, the first step is to install and run the servers for CoreNLP and SimpleNLG.  The folder called `Server BAT files` contains two Windows batch files that I use to start the servers.  These batch files refer to specific locations in the file system, so you will need to modify them to reflect the location where you've installed the `.jar` files.  The SimpleNLG server requires two `.jar` files that are not included in the SimpleNLG distribution -- `hsqldb_6148.jar` and `lexAccess2013dist.jar`.
