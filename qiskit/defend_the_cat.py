from qiskit import QuantumRegister, ClassicalRegister, QuantumCircuit, execute, Aer
# arbitrary state: https://quantumcomputing.stackexchange.com/questions/1413/how-to-create-an-arbitrary-state-in-qiskit-for-a-local-qasm-simulator

bits = ['0', '1']
all_inputs = []
for a in bits:
    for b in bits:
        for c in bits:
            for d in bits:
                all_inputs.append(a + b + c + d)

def color2(qreg, circuit):
    # (a)
    circuit.cx(qreg[0], qreg[5])
    circuit.cx(qreg[2], qreg[5])
    # (b)
    circuit.cx(qreg[0], qreg[6])
    circuit.cx(qreg[3], qreg[6])
    # (c)
    circuit.cx(qreg[1], qreg[7])
    circuit.cx(qreg[2], qreg[7])
    # (d)
    circuit.ccx(qreg[5], qreg[6], qreg[8])
    circuit.ccx(qreg[7], qreg[8], qreg[4])

for input in all_inputs:
    qreg = QuantumRegister(9)
    creg = ClassicalRegister(1)
    circuit = QuantumCircuit(qreg, creg)

    # initialize the inputs
    for i in range(4):
        if input[i] == '1':
            circuit.x(qreg[3 - i])

    color2(qreg, circuit)
    circuit.measure(qreg[4], creg)
    # print(circuit.draw(output='text'))

    job = execute(circuit, Aer.get_backend('qasm_simulator'), shots=1)
    counts = job.result().get_counts(circuit)
    for outcome in counts:
        print("(input vertices) |{}> --> |{}> (output)".format(input, outcome))
