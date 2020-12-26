from unity_handler import UnityHandler
from defend_the_cat import sample_outputs

unity_handler = UnityHandler()
while True:
    unity_handler.receive()
    sample_outputs(unity_handler.qbit, unity_handler.circuit)
