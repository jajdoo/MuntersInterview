# Giphy Integration
### Contract first 
I start with defining contract packages aiming to create signatures that follow conventions rather than shape those interfaces after implementation. 

I prefer signatures that are unlikely to break in the future even when things could change 
example: 
- Objects as input and output instead adding more function parameters later
- `Task` return types where implemtations are likely to require IO.

### Layered Structure
My typical structure when building servers is to separate the code into 3 layers:
1. <u>Transport</u>: entry to the server, typically http controllers, sanitizers, error handlers etc.. 
1. <u>Managers</u>: handles the business logic, I aim to make this layer as "pure" as possible, delegating calls to other managers or resource accessors.
1. <u>Resource Accessors</u>: abstracts access to specific resources and translate them to data objects that are defined in their contract without exposing specific implementation details.

