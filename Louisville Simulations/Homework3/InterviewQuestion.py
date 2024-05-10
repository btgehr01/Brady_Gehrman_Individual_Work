
import random
import matplotlib.pyplot as plt

def Scenario1(num_candidates):
    num_questions = 3
    num_choices = 5
    
    candidate_scores = []

    for candidate in range(num_candidates):
        questionsRight = 0
        for question in range(num_questions):
            # Randomly select a candidate's choice
            candidate_choice = random.randint(1, num_choices)
            
            # Check if the candidate's choice matches the last answer
            if candidate_choice == num_choices:
                questionsRight += 1  # Increment for correct answer
        
        candidate_scores.append((questionsRight / num_questions) * 100)

    mean_score = sum(candidate_scores) / len(candidate_scores)
    rounded_mean_score = round(mean_score, 2)
    print("Mean Score S1: " + str(rounded_mean_score) + "%")

    return rounded_mean_score

def Scenario2(num_candidates):
    num_questions = 3
    num_choices = 5
    
    candidate_scores = []

    for candidate in range(num_candidates):
        questionsRight = 0
        for question in range(num_questions):
            # Randomly select a candidate's choice
            candidate_choice = random.randint(1, num_choices)
            # Randomly select the correct answer (HR)
            right_answer = random.randint(1, num_choices)
        
            # Check if the candidate's choice matches the correct answer
            if candidate_choice == right_answer:
                questionsRight += 1  # Increment for correct answer
        
        candidate_scores.append((questionsRight / num_questions) * 100)

    mean_score = sum(candidate_scores) / len(candidate_scores)
    rounded_mean_score = round(mean_score, 2)
    print("Mean Score S2: " + str(rounded_mean_score) + "%")

    return rounded_mean_score



# print([round(score, 2) for score in Scenario1(10)])
# print([round(score, 2) for score in Scenario1(10)])



candidate_values = [100, 1000, 10000, 100000, 1000000, 10000000]
mean_scores_list1 = []
mean_scores_list2 = []
for value in candidate_values:
    mean_scores_list1.append(Scenario1(value))
    mean_scores_list2.append(Scenario2(value))\
    
# Plotting
plt.figure(figsize=(10, 6))

plt.plot(candidate_values, mean_scores_list1, label='Scenario 1', marker='o')
plt.plot(candidate_values, mean_scores_list2, label='Scenario 2', marker='x')

plt.xscale('log')  # Set logarithmic scale for x-axis
plt.xlabel('Number of Candidates')
plt.ylabel('Mean Score (%)')
plt.title('Mean Scores for Different Numbers of Candidates')
plt.legend()
plt.grid(True)
plt.show()
plt.savefig('Interview.png')





