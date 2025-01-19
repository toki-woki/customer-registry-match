# Company Receiving Capabilities
A simple tool to match your customer list and check what document/types they can receive when it comes to electronic invoices, orders etc.

Added an 'ExampleApp' that takes a pre-defined .csv with this structure:
```
CustomerNumber;Identifier;CustomerName
```

And returns a .csv with these parameters of all matches:
```
CustomerNumber;Identifier;CustomerName;Operator;DocumentType;FoundIdentifier
```

# Supported Networks:
- NEA (https://www.neaeregister.se)
