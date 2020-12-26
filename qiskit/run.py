from unity_handler import UnityHandler
from defend_the_cat import sample_outputs

unity_handler = UnityHandler()
while True:
    try:
        unity_handler.receive()
        results = sample_outputs(unity_handler.qbit, unity_handler.circuit)
        unity_handler.send(results)
    except:
        print("got exception")
        unity_handler = UnityHandler()
