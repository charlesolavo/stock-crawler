import flask
import random

app = flask.Flask(__name__)

class Stock:    
    def __init__(self, company, value):
        self.company = company
        self.value = value

companies = ['Empresa A', 'Empresa B', 'Empresa C', 'Empresa D', 'Empresa E']

@app.route('/', methods=['GET'])
def home():
    stocks = []
    for company in companies:
        stocks.append(Stock(company, round(random.uniform(0, 1000), 2)))
    
    return flask.render_template('index.html', stocks=stocks)

if __name__ == '__main__':
    app.run()